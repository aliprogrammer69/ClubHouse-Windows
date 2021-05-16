using System.Collections.Generic;
using System.Threading.Tasks;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;

namespace ClubHouse.Domain.Services {
    public interface IProfileService {
        Task<object> GetReleaseNotes();
        Task<CheckWaitListStatusResponse> CheckWaitlistStatus();
        Task<object> AddEmail(string email);
        Task<UpdateProfilePhotoResponse> UpdatePhoto(string filePath);
        Task<BaseResponse> UpdateUsername(string username);
        Task<BaseResponse> UpdateName(string name);
        Task<object> UpdateDisplayname(string name);
        Task<BaseResponse> UpdateBio(string bio);
        Task<object> UpdateTwitterUsername(string username, string twitterToken, string twitterSecret);
        Task<object> UpdateInstagramUsername(string code);
        Task<SuggestedUsersResponse> SuggestedUsers(SuggestedUsersRequest request);
        Task<object> GetSettings();
        Task<GetProfileResponse> GetInfo(bool returnBlockedIds = false, string timezoneIdentifier = "Asia/Tehran", bool returnFollowingIds = false);
        Task<NotificationsResponse> GetNotifications(PagingRequest request);
        Task<object> GetActionableNotifications();
        Task<object> IgnoreActionableNotifications(long actionableNotificationId);
        Task<object> GetOnlineFriends();

        Task<object> AddTopicInterest(long topicId);
        Task<object> AddClubInterest(long clubId);
        Task<object> RemoveTopicInterest(long topicId);
        Task<object> RemoveClubInterest(long clubId);

        Task<object> GetSuggestedInvites(long? clubId = null, bool uploadContacts = true, IEnumerable<object> contacts = null);
        Task<object> GetSuggestedInvites(bool uploadContacts = true, IEnumerable<object> contacts = null);
        Task<BaseResponse> InviteToApp(string name, string phoneNumber, string message = null);
        Task<object> InviteFromWaitlist(long userId);

        Task<object> RefreshToken(string refreshToken);
        Task<object> UserLog(object activity);
        Task<object> ReportIncident(long userId, string channel, object incidentType, string incidentDescription, string email);
    }
}
