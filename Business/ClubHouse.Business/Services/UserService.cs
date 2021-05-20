using ClubHouse.Common;
using ClubHouse.Domain;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;

using ClubHouse.Domain.Services.Common;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClubHouse.Business.Services {
    public class UserService : IUserService {
        private readonly IHttpClient _httpClient;

        public UserService(IHttpClient httpClient) {
            _httpClient = httpClient;
        }

        public Task<BaseResponse> Follow(long userId, int source = 4) =>
            _httpClient.PostAsync<object, BaseResponse>($"{APIConsts.API_URL}/follow", new {
                user_id = userId,
                source
            });

        public Task<object> Follow(IEnumerable<long> userId, int source = 7) =>
            _httpClient.PostAsync<object, object>($"{APIConsts.API_URL}/follow", new {
                user_id = userId,
                source
            });

        public Task<object> Block(long userId) => throw new NotImplementedException();

        public Task<BaseResponse> Unfollow(long userId) =>
            _httpClient.PostAsync<object, BaseResponse>($"{APIConsts.API_URL}/unfollow", new {
                user_id = userId
            });

        public Task<GetUserResponse> Get(long userId) =>
            _httpClient.PostAsync<object, GetUserResponse>($"{APIConsts.API_URL}/get_profile", new {
                user_id = userId
            });

        public Task<UserSearchResponse> GetFollowers(UserPagingRequest request) =>
            _httpClient.GetAsync<UserSearchResponse>($"{APIConsts.API_URL}/get_followers?{request}");

        public Task<UserSearchResponse> GetFollowing(UserPagingRequest request) =>
            _httpClient.GetAsync<UserSearchResponse>($"{APIConsts.API_URL}/get_following?{request}");

        public Task<UserSearchResponse> GetMutualFollows(UserPagingRequest request) =>
            _httpClient.GetAsync<UserSearchResponse>($"{APIConsts.API_URL}/get_mutual_follows?{request}");

        public Task<object> IgnoreInSuggestion(long userId) => throw new NotImplementedException();

        public Task<UserSearchResponse> Search(UserSearchRequest request) =>
            _httpClient.PostAsync<UserSearchRequest, UserSearchResponse>($"{APIConsts.API_URL}/search_users", request);

        public Task<object> SimilarUsers(long userId) => throw new NotImplementedException();

        public Task<object> Unblock(long userId) => throw new NotImplementedException();


        public Task<object> UpdateNotificationFrequency(long userId, NotificationFrequencyType notificationType = NotificationFrequencyType.Sometimes) => throw new NotImplementedException();

        public Task<NotificationsResponse> GetNotifications(PagingRequest request) =>
            _httpClient.GetAsync<NotificationsResponse>($"{APIConsts.API_URL}/get_notifications?{request}");
    }
}
