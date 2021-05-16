using System;
using System.Threading.Tasks;
using ClubHouse.Common;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;
using ClubHouse.Domain.Services.Common;

namespace ClubHouse.Business.Services {
    public class EventService : IEventService {
        private readonly IHttpClient _httpClient;

        public EventService(IHttpClient httpClient) {
            _httpClient = httpClient;
        }

        public Task<object> Create(EventRequest model) => throw new NotImplementedException();

        public Task<object> Delete(EventRequest model) => throw new NotImplementedException();

        public Task<object> Edit(EventRequest model) => throw new NotImplementedException();

        public Task<object> Get(EventRequest query) => throw new NotImplementedException();

        public Task<object> GetForUser() => throw new NotImplementedException();

        public Task<GetEventListResponse> GetList(GetEventListRequest request) =>
            _httpClient.GetAsync<GetEventListResponse>($"{APIConsts.API_URL}/get_events?{request}");


        public Task<object> GetToStart() => throw new NotImplementedException();
    }
}
