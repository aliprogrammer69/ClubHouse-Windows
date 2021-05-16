using System;
using System.Threading.Tasks;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Services;
using ClubHouse.Domain.Services.Common;

namespace ClubHouse.Business.Services {
    public class TopicService : ITopicService {
        private readonly IHttpClient _httpClient;

        public TopicService(IHttpClient httpClient) {
            _httpClient = httpClient;
        }

        public Task<object> Get() => throw new NotImplementedException();
        public Task<object> Get(long topicId) => throw new NotImplementedException();
        public Task<object> GetClubs(TopicPagingRequest request) => throw new NotImplementedException();
        public Task<object> GetUsers(TopicPagingRequest request) => throw new NotImplementedException();
    }
}
