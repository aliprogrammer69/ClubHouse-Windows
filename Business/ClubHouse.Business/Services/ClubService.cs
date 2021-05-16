using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClubHouse.Common;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;
using ClubHouse.Domain.Services.Common;

namespace ClubHouse.Business.Services {
    public class ClubService : IClubService {
        private readonly IHttpClient _httpClient;

        public ClubService(IHttpClient httpClient) {
            _httpClient = httpClient;
        }

        public Task<object> AcceptMemberInvite(long clubId, long? source_topic_id = null) => throw new NotImplementedException();
        public Task<object> AddAdmin(long clubId, long userId) => throw new NotImplementedException();
        public Task<object> AddMember(long clubId, long userId, string name, string phoneNumber, string message, object reason) => throw new NotImplementedException();
        public Task<object> AddTopic(long clubId, long topicId) => throw new NotImplementedException();
        public Task<object> ApproveNomination(long clubId, long sourceTopicId, long inviteNominationId) => throw new NotImplementedException();
        public Task<BaseResponse> Follow(long clubId, long? sourceTopicId = null) =>
            _httpClient.PostAsync<object, BaseResponse>($"{APIConsts.API_URL}/follow_club", new {
                club_id = clubId,
                source_topic_id = sourceTopicId
            });

        public Task<GetClubResponse> Get(long clubId, long? sourceTopicId = null) =>
            _httpClient.PostAsync<object, GetClubResponse>($"{APIConsts.API_URL}/get_club", new {
                club_id = clubId,
                source_topic_id = sourceTopicId
            });

        public Task<GetClubsResponse> Get(bool isStartableOnly = true) =>
            _httpClient.PostAsync<object, GetClubsResponse>($"{APIConsts.API_URL}/get_clubs", new {
                is_startable_only = isStartableOnly
            });

        public Task<GetClubMemebersResponse> GetMembers(GetClubMemberRequest request) =>
            _httpClient.GetAsync<GetClubMemebersResponse>($"{APIConsts.API_URL}/get_club_members?{request}");

        public Task<object> GetNominations(long clubId, long sourceTopicId) => throw new NotImplementedException();
        public Task<object> RejectNomination(long clubId, long sourceTopicId, long inviteNominationId) => throw new NotImplementedException();
        public Task<object> RemoveAdmin(long clubId, long userId) => throw new NotImplementedException();
        public Task<object> RemoveMember(long clubId, long userId) => throw new NotImplementedException();
        public Task<object> RemoveTopic(long clubId, long topicId) => throw new NotImplementedException();

        public Task<ClubSearchResponse> Search(ClubSearchRequest request) =>
            _httpClient.PostAsync<ClubSearchRequest, ClubSearchResponse>($"{APIConsts.API_URL}/search_clubs", request);

        public Task<object> SuggestedUsers(long clubId, bool upload_contacts = true, IDictionary<string, string> contacts = null) => throw new NotImplementedException();

        public Task<BaseResponse> Unfollow(long clubId, long? sourceTopicId = null) =>
            _httpClient.PostAsync<object, BaseResponse>($"{APIConsts.API_URL}/unfollow_club", new {
                club_id = clubId,
                source_topic_id = sourceTopicId
            });

        public Task<object> UpdateCommunity(long clubId, bool isCommunity) => throw new NotImplementedException();
        public Task<object> UpdateDescription(long clubId, string description) => throw new NotImplementedException();
        public Task<object> UpdateFollowAllowed(long clubId, bool isFollowAllowed = true) => throw new NotImplementedException();
        public Task<object> UpdateMembershipPrivate(long clubId, bool isMembershipPrivate) => throw new NotImplementedException();
        public Task<object> UpdateRules() => throw new NotImplementedException();
        public Task<object> UpdateTopics() => throw new NotImplementedException();
    }
}
