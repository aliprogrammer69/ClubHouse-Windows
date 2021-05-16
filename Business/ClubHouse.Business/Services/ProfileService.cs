using ClubHouse.Common;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;

using ClubHouse.Domain.Services.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClubHouse.Business.Services {
    public class ProfileService : IProfileService {
        private readonly IHttpClient _httpClient;

        public ProfileService(IHttpClient httpClient) {
            _httpClient = httpClient;
        }

        public Task<object> GetReleaseNotes() =>
            _httpClient.GetAsync<object>($"{APIConsts.API_URL}/get_release_notes");

        public Task<CheckWaitListStatusResponse> CheckWaitlistStatus() =>
            _httpClient.GetAsync<CheckWaitListStatusResponse>($"{APIConsts.API_URL}/check_waitlist_status");


        public Task<object> AddEmail(string email) =>
            _httpClient.PostAsync<object, object>($"{APIConsts.API_URL}/add_email", new {
                email
            });

        public async Task<UpdateProfilePhotoResponse> UpdatePhoto(string filePath) {
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{APIConsts.API_URL}/update_photo");
            MultipartFormDataContent content = new MultipartFormDataContent {
                { new ByteArrayContent(File.ReadAllBytes(filePath)), "file", Path.GetFileName(filePath) }
            };
            httpRequest.Content = content;
            return await _httpClient.SendAsync<UpdateProfilePhotoResponse>(httpRequest);
        }


        public Task<object> AddClubInterest(long clubId) => throw new NotImplementedException();

        public Task<object> AddTopicInterest(long topicId) => throw new NotImplementedException();

        public Task<object> GetActionableNotifications() => throw new NotImplementedException();

        public Task<GetProfileResponse> GetInfo(bool returnBlockedIds = false, string timezoneIdentifier = "Asia/Tehran", bool returnFollowingIds = false) =>
            _httpClient.PostAsync<object, GetProfileResponse>($"{APIConsts.API_URL}/me", new {
                return_blocked_ids = returnBlockedIds,
                timezone_identifier = timezoneIdentifier,
                return_following_ids = returnFollowingIds
            });

        public Task<NotificationsResponse> GetNotifications(PagingRequest request) =>
            _httpClient.GetAsync<NotificationsResponse>($"{APIConsts.API_URL}/get_notifications?{request}");

        public Task<object> GetOnlineFriends() => throw new NotImplementedException();

        public Task<object> GetSettings() => throw new NotImplementedException();

        public Task<object> GetSuggestedInvites(long? clubId = null, bool uploadContacts = true, IEnumerable<object> contacts = null) => throw new NotImplementedException();

        public Task<object> GetSuggestedInvites(bool uploadContacts = true, IEnumerable<object> contacts = null) => throw new NotImplementedException();

        public Task<object> IgnoreActionableNotifications(long actionableNotificationId) => throw new NotImplementedException();

        public Task<object> InviteFromWaitlist(long userId) => throw new NotImplementedException();

        public Task<BaseResponse> InviteToApp(string name, string phoneNumber, string message = null) =>
            _httpClient.PostAsync<object, BaseResponse>($"{APIConsts.API_URL}/invite_to_app", new {
                name,
                phone_number = phoneNumber,
                message
            });

        public Task<object> RefreshToken(string refreshToken) => throw new NotImplementedException();

        public Task<object> RemoveClubInterest(long clubId) => throw new NotImplementedException();

        public Task<object> RemoveTopicInterest(long topicId) => throw new NotImplementedException();

        public Task<object> ReportIncident(long userId, string channel, object incidentType, string incidentDescription, string email) => throw new NotImplementedException();

        public Task<SuggestedUsersResponse> SuggestedUsers(SuggestedUsersRequest request) =>
            _httpClient.GetAsync<SuggestedUsersResponse>($"{APIConsts.API_URL}/get_suggested_follows_all?{request}");

        public Task<BaseResponse> UpdateBio(string bio) =>
            _httpClient.PostAsync<object, BaseResponse>($"{APIConsts.API_URL}/update_bio", new {
                bio
            });

        public Task<object> UpdateDisplayname(string name) => throw new NotImplementedException();

        public Task<object> UpdateInstagramUsername(string code) => throw new NotImplementedException();

        public Task<BaseResponse> UpdateName(string name) =>
            _httpClient.PostAsync<object, BaseResponse>($"{APIConsts.API_URL}/update_name", new {
                name
            });


        public Task<object> UpdateTwitterUsername(string username, string twitterToken, string twitterSecret) => throw new NotImplementedException();

        public Task<BaseResponse> UpdateUsername(string username) =>
            _httpClient.PostAsync<object, BaseResponse>($"{APIConsts.API_URL}/update_username", new {
                username
            });

        public Task<object> UserLog(object activity) => throw new NotImplementedException();
    }
}
