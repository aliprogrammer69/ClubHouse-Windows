using System;
using System.Threading.Tasks;
using ClubHouse.Common;
using ClubHouse.Domain;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;
using ClubHouse.Domain.Services.Common;

namespace ClubHouse.Business.Services {
    public class ChannelService : IChannelService {
        private readonly IHttpClient _httpClient;

        public ChannelService(IHttpClient httpClient) {
            _httpClient = httpClient;
        }

        public Task<object> AcceptInviteToNewChannel(long channelInviteId) => throw new NotImplementedException();

        public Task<BaseResponse> AcceptSpeakerInvite(ChannelUserRequest request) =>
            _httpClient.PostAsync<ChannelUserRequest, BaseResponse>($"{APIConsts.API_URL}/accept_speaker_invite", request);

        public Task<ChannelPingResponse> ActivePing(string channel) =>
            _httpClient.PostAsync<object, ChannelPingResponse>($"{APIConsts.API_URL}/active_ping", new {
                channel
            });

        public Task<BaseResponse> AddModerator(ChannelUserRequest request) =>
            _httpClient.PostAsync<ChannelUserRequest, BaseResponse>($"{APIConsts.API_URL}/make_moderator", request);

        public Task<BaseResponse> BlockUser(ChannelUserRequest request) =>
            _httpClient.PostAsync<ChannelUserRequest, BaseResponse>($"{APIConsts.API_URL}/block_from_channel", request);

        public Task<object> CancelInviteToNewChannel(long channelInviteId) => throw new NotImplementedException();

        public Task<CreateChannelResponse> Create(CreateChannelRequest request) =>
            _httpClient.PostAsync<CreateChannelRequest, CreateChannelResponse>($"{APIConsts.API_URL}/create_channel", request);

        public Task<object> End(string channel) => throw new NotImplementedException();

        public Task<GetChannelsResponse> Get() =>
            _httpClient.GetAsync<GetChannelsResponse>($"{APIConsts.API_URL}/get_channels");

        public Task<GetChannelResponse> Get(string channel) =>
            _httpClient.PostAsync<object, GetChannelResponse>($"{APIConsts.API_URL}/get_channel", new {
                channel
            });

        public Task<CreateChannelTargetsResponse> GetCreateChannelTargets() =>
            _httpClient.PostAsync<object, CreateChannelTargetsResponse>($"{APIConsts.API_URL}/get_create_channel_targets", new { });

        public Task<object> GetSuggestedSpeakers(string channel) => throw new NotImplementedException();

        public Task<object> GetWelcome() => throw new NotImplementedException();

        public Task<BaseResponse> Hide(string channel, bool hide = true) =>
            _httpClient.PostAsync<object, BaseResponse>($"{APIConsts.API_URL}/hide_channel", new {
                channel,
                hide
            });

        public Task<BaseResponse> InviteSpeaker(ChannelUserRequest request) =>
            _httpClient.PostAsync<ChannelUserRequest, BaseResponse>($"{APIConsts.API_URL}/invite_speaker", request);

        public Task<InviteToJoinChannelResponse> InviteToJoin(ChannelUserRequest request) =>
            _httpClient.PostAsync<ChannelUserRequest, InviteToJoinChannelResponse>($"{APIConsts.API_URL}/invite_to_existing_channel", request);

        public Task<object> InviteToNewChannel(ChannelUserRequest request) => throw new NotImplementedException();

        public Task<JoinChannelResponse> Join(JoinChannelRequest request) =>
            _httpClient.PostAsync<JoinChannelRequest, JoinChannelResponse>($"{APIConsts.API_URL}/join_channel", request);

        public Task<BaseResponse> Leave(string channel) =>
            _httpClient.PostAsync<object, BaseResponse>($"{APIConsts.API_URL}/leave_channel", new {
                channel
            });

        public Task<object> MakePublic(string channel) => throw new NotImplementedException();

        public Task<object> MakeSocial(string channel) => throw new NotImplementedException();

        public Task<BaseResponse> MuteSpeaker(ChannelUserRequest request) =>
            _httpClient.PostAsync<ChannelUserRequest, BaseResponse>($"{APIConsts.API_URL}/mute_speaker", request);

        public Task<BaseResponse> RaiseHands(RaiseHandsRequest request) =>
            _httpClient.PostAsync<RaiseHandsRequest, BaseResponse>($"{APIConsts.API_URL}/audience_reply", request);

        public Task<object> RejectInviteToNewChannel(long channelInviteId) => throw new NotImplementedException();

        public Task<BaseResponse> RejectSpeakerInvite(ChannelUserRequest request) =>
            _httpClient.PostAsync<ChannelUserRequest, BaseResponse>($"{APIConsts.API_URL}/reject_speaker_invite", request);

        public Task<object> RejectWelcome() => throw new NotImplementedException();

        public Task<object> SetRaiseHandSetting(string channel, bool isEnabled = true, handraisePermission handraisePermission = handraisePermission.Everyone) => throw new NotImplementedException();

        public Task<BaseResponse> UninviteSpeaker(ChannelUserRequest request) =>
            _httpClient.PostAsync<ChannelUserRequest, BaseResponse>($"{APIConsts.API_URL}/uninvite_speaker", request);

        public Task<object> UpdateFlag(string channel, bool visibility, object flagTitle, object unflagTitle) => throw new NotImplementedException();

        public Task<object> UpdateSkintone(int skintone = 1) => throw new NotImplementedException();
    }
}
