using System.Collections.Generic;
using System.Threading.Tasks;

using ClubHouse.Domain.Models;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;

namespace ClubHouse.Domain.Services {
    public interface IUserService {
        Task<BaseResponse> Follow(long userId, int source = 4);
        Task<object> Follow(IEnumerable<long> userId, int source = 7);

        Task<BaseResponse> Unfollow(long userId);
        Task<object> Block(long userId);
        Task<object> Unblock(long userId);

        Task<object> UpdateNotificationFrequency(long userId, NotificationFrequencyType notificationType = NotificationFrequencyType.Sometimes);
        Task<NotificationsResponse> GetNotifications(PagingRequest request);
        Task<object> SimilarUsers(long userId);
        Task<object> IgnoreInSuggestion(long userId);

        Task<GetUserResponse> Get(long userId);
        Task<UserSearchResponse> GetFollowing(UserPagingRequest request);
        Task<UserSearchResponse> GetFollowers(UserPagingRequest request);
        Task<UserSearchResponse> GetMutualFollows(UserPagingRequest request);

        Task<UserSearchResponse> Search(UserSearchRequest request);
    }
}
