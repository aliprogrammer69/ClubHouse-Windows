using System.Collections.Generic;
using System.Threading.Tasks;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;

namespace ClubHouse.Domain.Services {
    public interface IChannelService {
        Task<object> GetWelcome();
        Task<object> RejectWelcome();

        Task<GetChannelsResponse> Get();
        Task<GetChannelResponse> Get(string channel);
        Task<BaseResponse> Hide(string channel, bool hide = true);

        Task<CreateChannelTargetsResponse> GetCreateChannelTargets();
        Task<CreateChannelResponse> Create(CreateChannelRequest request);
        Task<JoinChannelResponse> Join(JoinChannelRequest request);
        Task<ChannelPingResponse> ActivePing(string channel);
        Task<BaseResponse> Leave(string channel);
        Task<object> End(string channel);

        Task<BaseResponse> RaiseHands(RaiseHandsRequest request);
        Task<object> SetRaiseHandSetting(string channel, bool isEnabled = true, handraisePermission handraisePermission = handraisePermission.Everyone);
        Task<object> UpdateSkintone(int skintone = 1);
        Task<BaseResponse> AcceptSpeakerInvite(ChannelUserRequest request);
        Task<BaseResponse> RejectSpeakerInvite(ChannelUserRequest request);
        Task<BaseResponse> InviteSpeaker(ChannelUserRequest request);
        Task<BaseResponse> UninviteSpeaker(ChannelUserRequest request);
        Task<BaseResponse> MuteSpeaker(ChannelUserRequest request);
        Task<object> GetSuggestedSpeakers(string channel);
        Task<BaseResponse> AddModerator(ChannelUserRequest request);

        Task<InviteToJoinChannelResponse> InviteToJoin(ChannelUserRequest request);
        Task<object> InviteToNewChannel(ChannelUserRequest request);
        Task<object> AcceptInviteToNewChannel(long channelInviteId);
        Task<object> RejectInviteToNewChannel(long channelInviteId);
        Task<object> CancelInviteToNewChannel(long channelInviteId);

        /// <summary>
        /// Everyone can join the channel.
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        Task<object> MakePublic(string channel);

        /// <summary>
        /// Only people who user follows can join the channel.
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        Task<object> MakeSocial(string channel);


        /// <summary>
        /// Remove the user from the channel. The user will not be able to re-join.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResponse> BlockUser(ChannelUserRequest request);

        Task<object> UpdateFlag(string channel, bool visibility, object flagTitle, object unflagTitle);

    }
}
