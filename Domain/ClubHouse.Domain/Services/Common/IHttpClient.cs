using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ClubHouse.Domain.Models.Common;

namespace ClubHouse.Domain.Services.Common {
    public interface IHttpClient {
        HttpMessageHandler HttpMessageHandler { get; set; }
        TimeSpan? Timeout { get; set; }

        Task<Tout> PostAsync<Tin, Tout>(string url, Tin model, IEnumerable<KeyValuePair<string, string>> headers = null);
        Task<HttpResponse<Tout>> PostAsync<Tin, Tout>(string url, Tin model, FillingInfo fillingInfo, IEnumerable<KeyValuePair<string, string>> headers = null);

        Task<Tout> GetAsync<Tout>(string url, IEnumerable<KeyValuePair<string, string>> headers = null);
        Task<HttpResponse<Tout>> GetAsync<Tout>(string url, FillingInfo fillingInfo, IEnumerable<KeyValuePair<string, string>> headers = null);

        Task<Tout> SendAsync<Tout>(HttpRequestMessage request);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}
