using System.Collections.Generic;
using System.Threading.Tasks;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;

namespace ClubHouse.Domain.Services {
    public interface IClubService {
        Task<BaseResponse> Follow(long clubId, long? sourceTopicId = null);
        Task<BaseResponse> Unfollow(long clubId, long? sourceTopicId = null);

        Task<object> SuggestedUsers(long clubId, bool upload_contacts = true, IDictionary<string, string> contacts = null);

        Task<GetClubResponse> Get(long clubId, long? sourceTopicId = null);
        Task<GetClubMemebersResponse> GetMembers(GetClubMemberRequest request);

        Task<ClubSearchResponse> Search(ClubSearchRequest request);
        Task<GetClubsResponse> Get(bool isStartableOnly = true);

        Task<object> AddAdmin(long clubId, long userId);
        Task<object> RemoveAdmin(long clubId, long userId);
        Task<object> RemoveMember(long clubId, long userId);
        Task<object> AcceptMemberInvite(long clubId, long? source_topic_id = null);
        Task<object> AddMember(long clubId, long userId, string name, string phoneNumber, string message, object reason);

        Task<object> GetNominations(long clubId, long sourceTopicId);
        Task<object> ApproveNomination(long clubId, long sourceTopicId, long inviteNominationId);
        Task<object> RejectNomination(long clubId, long sourceTopicId, long inviteNominationId);

        Task<object> AddTopic(long clubId, long topicId);
        Task<object> RemoveTopic(long clubId, long topicId);

        Task<object> UpdateFollowAllowed(long clubId, bool isFollowAllowed = true);
        Task<object> UpdateMembershipPrivate(long clubId, bool isMembershipPrivate);
        Task<object> UpdateCommunity(long clubId, bool isCommunity);
        Task<object> UpdateDescription(long clubId, string description);
        Task<object> UpdateRules();
        Task<object> UpdateTopics();

    }
}
