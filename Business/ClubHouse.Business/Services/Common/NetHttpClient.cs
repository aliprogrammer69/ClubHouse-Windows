using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ClubHouse.Domain.Models.Common;
using ClubHouse.Domain.Services.Common;

namespace ClubHouse.Business.Services.Common {
    public class NetHttpClient : IHttpClient {
        private readonly ISerializer _serializer;
        public NetHttpClient(ISerializer serializer, HttpMessageHandler httpMessageHandler = null) {
            _serializer = serializer;
            _httpMessageHandler = httpMessageHandler ?? new HttpClientHandler();
        }

        private HttpMessageHandler _httpMessageHandler;
        public HttpMessageHandler HttpMessageHandler {
            get => _httpMessageHandler;
            set => _httpMessageHandler = value ?? throw new Exception("HttpMessageHandler can't be null.");
        }

        public TimeSpan? Timeout { get; set; }

        public async Task<Tout> GetAsync<Tout>(string url, IEnumerable<KeyValuePair<string, string>> headers = null) {
            using HttpClient client = new HttpClient(_httpMessageHandler, false);
            if (Timeout != null) {
                client.Timeout = Timeout.Value;
            }
            ApplyHeadersToRequest(client, headers);
            HttpResponseMessage apiResult = await client.GetAsync(url);
            string resultString = await apiResult.Content.ReadAsStringAsync();
            return _serializer.SafeDeserialize<Tout>(resultString);
        }

        public async Task<HttpResponse<Tout>> GetAsync<Tout>(string url, FillingInfo fillingInfo, IEnumerable<KeyValuePair<string, string>> headers = null) {
            using HttpClient client = new HttpClient(_httpMessageHandler, false);
            if (Timeout != null) {
                client.Timeout = Timeout.Value;
            }
            ApplyHeadersToRequest(client, headers);
            HttpResponseMessage apiResult = await client.GetAsync(url);
            return FillHttpResponse<Tout>(apiResult, fillingInfo);
        }

        public async Task<Tout> PostAsync<Tin, Tout>(string url, Tin model, IEnumerable<KeyValuePair<string, string>> headers = null) {
            using HttpClient client = new HttpClient(_httpMessageHandler, false);
            if (Timeout != null) {
                client.Timeout = Timeout.Value;
            }
            ApplyHeadersToRequest(client, headers);
            HttpContent content = new StringContent(_serializer.Serialize(model), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage apiResult = await client.PostAsync(url, content);
            string resultString = await apiResult.Content.ReadAsStringAsync();
            return _serializer.SafeDeserialize<Tout>(resultString);
        }

        public async Task<HttpResponse<Tout>> PostAsync<Tin, Tout>(string url, Tin model, FillingInfo fillingInfo, IEnumerable<KeyValuePair<string, string>> headers = null) {
            using HttpClient client = new HttpClient(_httpMessageHandler, false);
            if (Timeout != null) {
                client.Timeout = Timeout.Value;
            }
            ApplyHeadersToRequest(client, headers);
            HttpContent content = new StringContent(_serializer.Serialize(model), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage apiResult = await client.PostAsync(url, content);
            return FillHttpResponse<Tout>(apiResult, fillingInfo);
        }

        public async Task<Tout> SendAsync<Tout>(HttpRequestMessage request) {
            using HttpClient client = new HttpClient(_httpMessageHandler, false);
            if (Timeout != null) {
                client.Timeout = Timeout.Value;
            }
            HttpResponseMessage resposne = await client.SendAsync(request);
            string jsonResponse = await resposne.Content.ReadAsStringAsync();
            return _serializer.SafeDeserialize<Tout>(jsonResponse);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) {
            using HttpClient client = new HttpClient(_httpMessageHandler, false);
            if (Timeout != null) {
                client.Timeout = Timeout.Value;
            }
            return await client.SendAsync(request);
        }

        protected virtual HttpResponse<T> FillHttpResponse<T>(HttpResponseMessage httpResponseMessage, FillingInfo fillingInfo) => new HttpResponse<T>() {
            StatusCode = (fillingInfo & FillingInfo.StatusCode) == FillingInfo.StatusCode ? httpResponseMessage.StatusCode : 0,
            Headers = (fillingInfo & FillingInfo.Headers) == FillingInfo.Headers ? ExtractHeaders(httpResponseMessage) : null,
            Body = (fillingInfo & FillingInfo.Body) == FillingInfo.Body ?
                    _serializer.SafeDeserialize<T>(httpResponseMessage.Content?.ReadAsStringAsync().Result) : default
        };

        #region Private Methods
        private void ApplyHeadersToRequest(HttpClient client, IEnumerable<KeyValuePair<string, string>> headers) {
            if (headers == null || !headers.Any()) {
                return;
            }

            foreach (KeyValuePair<string, string> header in headers)
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        private IEnumerable<KeyValuePair<string, IEnumerable<string>>> ExtractHeaders(HttpResponseMessage httpResponseMessage) {
            return httpResponseMessage.Headers.Select(h => new KeyValuePair<string, IEnumerable<string>>(h.Key, h.Value));
        }
        #endregion
    }
}
