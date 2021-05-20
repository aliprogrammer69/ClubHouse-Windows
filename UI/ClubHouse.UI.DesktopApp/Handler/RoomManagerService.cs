using agorartc;

using ClubHouse.Common;
using ClubHouse.Domain.Models;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;
using ClubHouse.Domain.Services.Common;
using ClubHouse.UI.DesktopApp.Models;
using ClubHouse.UI.DesktopApp.ViewModels;
using ClubHouse.UI.DesktopApp.Views;

using MaterialDesignThemes.Wpf;

using PubnubApi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Unity;

namespace ClubHouse.UI.DesktopApp.Handler {
    public class RoomManagerService : BaseViewModel, IDisposable {
        private readonly IChannelService _channelService;
        private readonly IAccountService _accountService;
        private readonly IMessageService _messageService;
        private readonly AgoraRtcEngine _rtcEngine;
        private readonly ISerializer _serializer;
        private readonly IUnityContainer _unityContainer;
        private readonly ClubHouseEventHandler eventHandler;

        private CancellationTokenSource _cancelationTokenSource;
        private CancellationTokenSource _raiseHandCancelationTokenSource;

        private BaseChannelResponse channelInfo;
        private bool raisingHand;
        private RoomUserCollections joinedUsers;
        private bool speakerMuted;
        private bool microphoneMuted;
        private BindableChannelUser currentUserInfo;
        private Pubnub pubnub;

        public delegate void Join(BaseChannelResponse channel);
        public event Join OnJoined;
        public event Join OnLeave;

        public RoomManagerService(IChannelService channelService,
                             IAccountService accountService,
                             IMessageService messageService,
                             AgoraRtcEngine rtcEngine,
                             ISerializer serializer,
                             IUnityContainer unityContainer) {
            _channelService = channelService;
            _accountService = accountService;
            _messageService = messageService;
            _serializer = serializer;
            _unityContainer = unityContainer;
            JoinedUsers = new RoomUserCollections();
            eventHandler = new ClubHouseEventHandler(JoinedUsers);
            //eventHandler.OnRefereshRoom += GetDetail;

            _rtcEngine = rtcEngine;

            _rtcEngine.Initialize(APIConsts.AGORA_KEY, AREA_CODE.AREA_CODE_GLOBAL);
            _rtcEngine.InitEventHandler(eventHandler);

            _cancelationTokenSource = new CancellationTokenSource();
            _raiseHandCancelationTokenSource = new CancellationTokenSource();
        }

        public void Dispose() {
            DisposeRoom();
            _cancelationTokenSource.Dispose();
            _raiseHandCancelationTokenSource.Dispose();
        }

        public BaseChannelResponse ChannelInfo { get => channelInfo; set => SetProperty(ref channelInfo, value); }
        public RoomUserCollections JoinedUsers { get => joinedUsers; set => SetProperty(ref joinedUsers, value); }
        public BindableChannelUser CurrentUserInfo { get => currentUserInfo; set => SetProperty(ref currentUserInfo, value); }
        public bool RaisingHand { get => raisingHand; set => SetProperty(ref raisingHand, value); }
        public bool SpeakerMuted { get => speakerMuted; set => SetProperty(ref speakerMuted, value); }
        public bool MicrophoneMuted { get => microphoneMuted; set => SetProperty(ref microphoneMuted, value); }
        public bool Joined { get; private set; }

        public IEnumerable<BindableChannelUser> Speakers { get => JoinedUsers.Where(u => u.Is_speaker); }

        public async Task<InitChannelResponse> JoinRoom(string channel) {
            if (ChannelInfo != null && (string.IsNullOrEmpty(channel) || channel != ChannelInfo.Channel))
                await LeaveRoom();

            JoinChannelResponse apiResult = await _channelService.Join(new JoinChannelRequest {
                Channel = channel
            });
            if (string.IsNullOrEmpty(apiResult?.Token) || !apiResult.Success) {
                _messageService.Show(apiResult?.Error_message ?? "Failed to join room");
            }

            return apiResult;
        }

        public void InitRoom(InitChannelResponse channelInfo) {
            if (string.IsNullOrEmpty(channelInfo?.Token) || !channelInfo.Success) {
                _messageService.Show("Failed To open room.");
                return;
            }

            var joinResult = _rtcEngine.JoinChannel(channelInfo.Token, channelInfo.Channel, "", (uint)_accountService.CurrentConfig.UserId.Value);
            if (joinResult != ERROR_CODE.ERR_OK) {
                _messageService.Show($"Failed to join RTCEngine. Error code is {joinResult}");
                return;
            }

            ChannelInfo = channelInfo;

            JoinedUsers.Clear();
            JoinedUsers.AddRange(channelInfo.Users.Select(u => new BindableChannelUser(u)));
            JoinedUsers.RefereshView();
            CurrentUserInfo = JoinedUsers.Find(_accountService.CurrentConfig.UserId ?? 0);

            InitPubNub(channelInfo);

            _cancelationTokenSource.Dispose();
            _cancelationTokenSource = new CancellationTokenSource();
            _ = Task.Run(ActivePing);

            Joined = true;
            OnJoined?.Invoke(channelInfo);
        }

        private void InitPubNub(InitChannelResponse channelInfo) {
            if (!channelInfo.Pubnub_enable) {
                return;
            }

            PNConfiguration pnConfiguration = new() {
                SubscribeKey = channelInfo.Pubnub_sub_key ?? APIConsts.PUBNUB_SUB_KEY,
                PublishKey = channelInfo.Pubnub_pub_key ?? APIConsts.PUBNUB_PUB_KEY,
                Uuid = _accountService.CurrentConfig.UserId.ToString(),
                Origin = channelInfo.Pubnub_origin,
                AuthKey = channelInfo.Pubnub_token,
                PresenceTimeout = channelInfo.Pubnub_heartbeat_value
            };
            pnConfiguration.SetPresenceTimeoutWithCustomInterval(channelInfo.Pubnub_heartbeat_value, channelInfo.Pubnub_heartbeat_interval);
            pubnub = new Pubnub(pnConfiguration);

            pubnub.AddListener(new SubscribeCallbackExt(
                PubNubMessageActions,
                delegate (Pubnub pnObj, PNPresenceEventResult presenceEvnt) {
                    if (presenceEvnt != null) {
                        Debug.WriteLine("Pubnub subscribeCallback received PNPresenceEventResult: " + presenceEvnt.Channel + " " + presenceEvnt.Occupancy + " " + presenceEvnt.Event);
                    }
                },
                delegate (Pubnub pnObj, PNStatus pnStatus) {
                    if (pnStatus.Category != PNStatusCategory.PNConnectedCategory) {
                        Debug.WriteLine($"Pubnub Error: {pnStatus.Category}, {pnStatus.ErrorData?.Information}");
                    }
                }));

            List<string> pubnubChannels = new() {
                "users." + CurrentUserInfo.User_id,
                "channel_user." + channelInfo.Channel + "." + CurrentUserInfo.User_id,
                "channel_all." + channelInfo.Channel,
            };
            if (CurrentUserInfo.Is_moderator) {
                pubnubChannels.Add("channel_speakers." + channelInfo.Channel);
            }
            pubnub.Subscribe<string>()
                .Channels(pubnubChannels.ToArray())
                .Execute();
        }

        public async void ChangeRaiseHand() {
            var apiResult = await _channelService.RaiseHands(new RaiseHandsRequest {
                Channel = ChannelInfo.Channel,
                Raise_hands = !RaisingHand,
                Unraise_hands = RaisingHand
            });
            if (apiResult == null || !apiResult.Success) {
                _messageService.Show($"Failed to raise hand. errorMessage is {apiResult.Error_message}");
                return;
            }

            RaisingHand = !RaisingHand;
            if (RaisingHand) {
                _raiseHandCancelationTokenSource.Dispose();
                _raiseHandCancelationTokenSource = new CancellationTokenSource();
                _ = Task.Run(CheckAcceptSpeakerInvition);
            }
            else {
                _raiseHandCancelationTokenSource.Cancel();
            }
        }

        public void ChangeSpeakerMute() {
            if (ChannelInfo == null)
                return;

            var result = _rtcEngine.MuteAllRemoteAudioStreams(!SpeakerMuted);
            if (result != ERROR_CODE.ERR_OK) {
                _messageService.Show($"Failed to mute the speaker. RTC engine returned {result} code");
                return;
            }

            SpeakerMuted = !SpeakerMuted;
        }

        public void ChangeMicrophoneMute() {
            if (ChannelInfo == null)
                return;

            var result = _rtcEngine.MuteLocalAudioStream(!MicrophoneMuted);
            if (result != ERROR_CODE.ERR_OK) {
                _messageService.Show($"Failed to mute the Mic. RTC engine returned {result} Code");
                return;
            }

            MicrophoneMuted = !MicrophoneMuted;
            var userInfo = JoinedUsers.Find(_accountService.CurrentConfig.UserId ?? 0);
            if (userInfo != null) {
                userInfo.Is_muted = MicrophoneMuted;
            }
        }

        public async void ChangeUserMicrophoneMuted(BindableChannelUser user) {
            if (ChannelInfo == null || user == null || CurrentUserInfo?.Is_moderator != true)
                return;

            var apiResult = await _channelService.MuteSpeaker(new ChannelUserRequest() {
                Channel = ChannelInfo.Channel,
                User_id = user.User_id
            });
            if (!apiResult.Success) {
                _messageService.Show($"{apiResult.Error_message}");
            }

            //var result = _rtcEngine.MuteRemoteAudioStream((uint)user.User_id, !user.Is_muted);
            //if (result != ERROR_CODE.ERR_OK) {
            //    _messageService.Show($"Failed to mute {user.Username}. RTC engine returned {result} code");
            //}
        }

        public async void InviteUserToSpeak(BindableChannelUser user) {
            if (ChannelInfo == null || user == null || !CurrentUserInfo?.Is_moderator != true)
                return;

            var apiResult = await _channelService.InviteSpeaker(new ChannelUserRequest() {
                Channel = ChannelInfo.Channel,
                User_id = user.User_id
            });
            if (!apiResult.Success) {
                _messageService.Show($"{apiResult.Error_message}");
            }
        }

        public async void MakeUserModerator(BindableChannelUser user) {
            if (ChannelInfo == null || user == null || !CurrentUserInfo?.Is_moderator != true)
                return;

            var apiResult = await _channelService.AddModerator(new ChannelUserRequest() {
                Channel = ChannelInfo.Channel,
                User_id = user.User_id
            });
            if (!apiResult.Success) {
                _messageService.Show($"{apiResult.Error_message}");
            }

            //GetDetail();
        }

        public async void MoveToAudiance(BindableChannelUser user) {
            if (ChannelInfo == null || user == null || CurrentUserInfo?.Is_moderator != true)
                return;

            var apiResult = await _channelService.UninviteSpeaker(new ChannelUserRequest() {
                Channel = ChannelInfo.Channel,
                User_id = user.User_id
            });
            if (!apiResult.Success) {
                _messageService.Show($"{apiResult.Error_message}");
            }

            //GetDetail();
        }

        public async Task<bool> LeaveRoom() {
            if (channelInfo == null) {
                return true;
            }

            var apiResult = await _channelService.Leave(channelInfo.Channel);
            if (apiResult == null || !apiResult.Success) {
                _messageService.Show($"Failed to leave channel. error message is {apiResult.Error_message}");
                return false;
            }

            DisposeRoom();
            Joined = false;
            OnLeave?.Invoke(channelInfo);
            return true;
        }

        private void DisposeRoom() {
            if (raisingHand) {
                _raiseHandCancelationTokenSource.Cancel();
            }

            LeavePubnub();
            _rtcEngine.LeaveChannel();
            _cancelationTokenSource.Cancel();
            ThreadManagerUtil.RunInUI(() => {
                ChannelInfo = null;
                JoinedUsers.Clear();
            });
        }

        private void LeavePubnub() {
            try {
                if (pubnub != null) {
                    pubnub.UnsubscribeAll<string>();
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
            pubnub = null;
        }

        private async Task ActivePing() {
            while (!_cancelationTokenSource.Token.IsCancellationRequested) {
                var pingResult = await _channelService.ActivePing(ChannelInfo.Channel);
                //if (pingResult != null && pingResult.Should_leave) {
                //    Console.WriteLine("[-] Should leave channel");
                //    await LeaveRoom();
                //    break;
                //}
                await Task.Delay(30000, _cancelationTokenSource.Token);
            }
        }

        public async void AcceptRaiseHand(BindableChannelUser user) {
            var apiResult = await _channelService.AcceptSpeakerInvite(new ChannelUserRequest {
                Channel = ChannelInfo.Channel,
                User_id = user.User_id
            });
            if (apiResult?.Success != true) {
                _messageService.Show(apiResult?.Error_message ?? "Accept raise hand error");
            }
        }

        public async void RejectRaiseHand(BindableChannelUser user) {
            var apiResult = await _channelService.RejectSpeakerInvite(new ChannelUserRequest {
                Channel = ChannelInfo.Channel,
                User_id = user.User_id
            });
            if (apiResult?.Success != true) {
                _messageService.Show(apiResult?.Error_message ?? "Accept raise hand error");
            }
        }


        private async Task CheckAcceptSpeakerInvition() {
            int tryCount = 0;
            while (tryCount < 10 && !_cancelationTokenSource.IsCancellationRequested && !_raiseHandCancelationTokenSource.Token.IsCancellationRequested) {
                var channelResult = await _channelService.Get(ChannelInfo.Channel);
                var acceptResult = await _channelService.AcceptSpeakerInvite(new ChannelUserRequest() {
                    Channel = ChannelInfo.Channel,
                    User_id = channelResult.Users.Select((Func<ChannelUser, long>)(u => (long)u.User_id)).FirstOrDefault(u => u != _accountService.CurrentConfig.UserId.Value)
                });
                if (acceptResult?.Success == true) {
                    Rejoin();
                    break;
                }
                await Task.Delay(10000, _raiseHandCancelationTokenSource.Token);
                tryCount++;
            }

            ThreadManagerUtil.RunInUI(() => {
                RaisingHand = false;
            });
        }

        public async void Rejoin() {
            var channel = ChannelInfo;

            _rtcEngine.LeaveChannel();

            JoinChannelResponse apiResult = await _channelService.Join(new JoinChannelRequest {
                Channel = channelInfo.Channel
            });
            if (string.IsNullOrEmpty(apiResult?.Token) || !apiResult.Success) {
                _messageService.Show("Failed To join channel.");
                return;
            }

            var joinResult = _rtcEngine.JoinChannel(apiResult.Token, apiResult.Channel, "", (uint)_accountService.CurrentConfig.UserId.Value);
            if (joinResult != ERROR_CODE.ERR_OK) {
                _messageService.Show($"Failed to join RTCEngine. Error code is {joinResult}");
                return;
            }

            RefereshUserView(apiResult.Users);
            ThreadManagerUtil.RunInUI(() => {
                ChannelInfo = apiResult;
            });
        }

        public async void GetDetail() {
            if (ChannelInfo == null)
                return;

            GetChannelResponse apiResult = await _channelService.Get(ChannelInfo.Channel);
            if (apiResult?.Users == null) {
                return;
            }

            RefereshUserView(apiResult.Users);
            ThreadManagerUtil.RunInUI(() => {
                ChannelInfo = apiResult;
            });
        }

        private void RefereshUserView(IEnumerable<ChannelUser> apiUsers) {
            for (int i = 0; i < apiUsers.Count(); i++) {
                var item = apiUsers.ElementAt(i);
                BindableChannelUser user = JoinedUsers.Count > i ? JoinedUsers.ElementAt(i) : null;
                if (user == null || user.User_id != item.User_id) {
                    user = JoinedUsers.Find(item.User_id);
                }

                if (user != null) {
                    if (user.Is_speaker != item.Is_speaker ||
                        (!item.Is_speaker && user.Is_followed_by_speaker != item.Is_followed_by_speaker))
                        ThreadManagerUtil.RunInUI(() => {
                            JoinedUsers.Remove(user);
                            JoinedUsers.Insert(i, new BindableChannelUser(item));
                        });
                    else if (!user.Equals(item))
                        ThreadManagerUtil.RunInUI(() => user.SetProperties(item));
                }
                else {
                    ThreadManagerUtil.RunInUI(() => JoinedUsers.Insert(i, new BindableChannelUser(item)));
                }
            }

            for (int i = 0; i < JoinedUsers.Count; i++) {
                var item = JoinedUsers[i];
                if (!apiUsers.Any(u => u.User_id == item.User_id)) {
                    ThreadManagerUtil.RunInUI(() => {
                        JoinedUsers.Remove(item);
                    });
                    i--;
                }
            }

            var userInfo = JoinedUsers.Find(_accountService.CurrentConfig.UserId ?? 0);
            ThreadManagerUtil.RunInUI(() => {
                CurrentUserInfo = userInfo;
            });
        }

        private void PubNubMessageActions(Pubnub pnObj, PNMessageResult<object> pubMsg) {
            var message = pubMsg?.Message?.ToString();
            if (string.IsNullOrEmpty(message)) {
                return;
            }

            var data = _serializer.Deserialize<PubnubResponse>(message);
            if (string.IsNullOrEmpty(data?.Channel) || data.Channel != ChannelInfo?.Channel) {
                return;
            }

            switch (data.Action) {
                case "join_channel":
                    if (data.User_profile == null) {
                        Debug.WriteLine($"Pubnub error: {message}");
                    }
                    else {
                        var user = JoinedUsers.Find(data.User_profile.User_id);
                        if (user == null) {
                            ThreadManagerUtil.RunInUI(() => {
                                JoinedUsers.Add(new BindableChannelUser(data.User_profile));
                            });
                        }
                        else {
                            ThreadManagerUtil.RunInUI(() => {
                                JoinedUsers.Remove(user);
                                user.SetProperties(data.User_profile);
                                JoinedUsers.Add(user);
                            });
                        }
                    }
                    break;
                case "leave_channel":
                    if (data.User_id <= 0) {
                        Debug.WriteLine($"Pubnub error: {message}");
                    }
                    else {
                        var user = JoinedUsers.Find(data.User_id);
                        if (user != null) {
                            ThreadManagerUtil.RunInUI(() => {
                                JoinedUsers.Remove(user);
                            });
                        }
                        else {
                            Debug.WriteLine($"User Offline: User {data.User_id} not found");
                        }
                    }
                    break;
                case "add_speaker":
                    if (data.User_profile == null) {
                        Debug.WriteLine($"Pubnub error: {message}");
                    }
                    else {
                        var user = JoinedUsers.Find(data.User_profile.User_id);
                        if (user != null) {
                            ThreadManagerUtil.RunInUI(() => {
                                JoinedUsers.Remove(user);
                                user.SetProperties(data.User_profile);
                                JoinedUsers.Add(user);
                            });
                        }
                        else {
                            ThreadManagerUtil.RunInUI(() => {
                                JoinedUsers.Add(new BindableChannelUser(data.User_profile));
                            });
                        }
                    }
                    break;
                case "make_moderator":
                    if (data.User_id <= 0) {
                        Debug.WriteLine($"Pubnub error: {message}");
                    }
                    else {
                        var user = JoinedUsers.Find(data.User_id);
                        if (user != null) {
                            ThreadManagerUtil.RunInUI(() => {
                                user.Is_moderator = true;
                            });
                        }
                        else {
                            Debug.WriteLine($"Pubnub user not found: {message}");
                        }
                    }
                    break;
                case "remove_speaker":
                    if (data.User_id <= 0) {
                        Debug.WriteLine($"Pubnub error: {message}");
                    }
                    else {
                        var user = JoinedUsers.Find(data.User_id);
                        if (user != null) {
                            ThreadManagerUtil.RunInUI(() => {
                                JoinedUsers.Remove(user);
                                user.Is_speaker = false;
                                JoinedUsers.Add(user);
                            });
                        }
                        else {
                            Debug.WriteLine($"Pubnub user not found: {message}");
                        }
                    }
                    break;
                case "update_speaker_call_status":
                    if (data.User_id <= 0) {
                        Debug.WriteLine($"Pubnub error: {message}");
                    }
                    else {
                        var user = JoinedUsers.Find(data.User_id);
                        if (user != null) {
                            ThreadManagerUtil.RunInUI(() => {
                                JoinedUsers.Remove(user);
                                user.Is_on_call = data.Is_on_call;
                                JoinedUsers.Add(user);
                            });
                        }
                        else {
                            Debug.WriteLine($"Pubnub user not found: {message}");
                        }
                    }
                    break;
                case "raise_hands":
                    if (data.User_profile == null) {
                        Debug.WriteLine($"Pubnub error: {message}");
                    }
                    else {
                        var user = JoinedUsers.Find(data.User_profile.User_id);
                        if (user == null) {
                            user = new BindableChannelUser(data.User_profile);
                            ThreadManagerUtil.RunInUI(() => {
                                JoinedUsers.Add(user);
                            });
                        }

                        ThreadManagerUtil.RunInUI(() => {
                            user.Raise_hands = true;
                        });
                    }
                    break;
                case "unraise_hands":
                    if (data.User_id <= 0) {
                        Debug.WriteLine($"Pubnub error: {message}");
                    }
                    else {
                        var user = JoinedUsers.Find(data.User_id);
                        if (user != null) {
                            ThreadManagerUtil.RunInUI(() => {
                                user.Raise_hands = false;
                            });
                        }
                    }
                    break;
                case "invite_speaker":
                    if (data.From_user_id <= 0) {
                        Debug.WriteLine($"Pubnub error: {message}");
                    }
                    else {
                        ThreadManagerUtil.RunInUI(async () => {
                            var view = _unityContainer.Resolve<JoinAsSpeakerView>();
                            if (view.DataContext is JoinAsSpeakerViewModel viewModel) {
                                viewModel.FromName = data.From_name;
                                viewModel.FromUserId = data.From_user_id;
                            }
                            var res = await DialogHost.Show(view);
                            if (res == null) {
                                await _channelService.RejectSpeakerInvite(new ChannelUserRequest {
                                    Channel = ChannelInfo.Channel,
                                    User_id = data.From_user_id
                                });
                            }
                        });
                    }
                    break;
                case "uninvite_speaker":
                    if (data.From_user_id <= 0) {
                        Debug.WriteLine($"Pubnub error: {message}");
                    }
                    else {
                        _messageService.Show($"{data.From_name} moved you to audiance");
                        ThreadManagerUtil.RunInUI(() => {
                            if (CurrentUserInfo != null)
                                CurrentUserInfo.Is_speaker = false;
                        });
                    }
                    break;
                case "mute_speaker":
                    _messageService.Show($"{data.From_name} muted you");
                    ThreadManagerUtil.RunInUI(() => {
                        if (!this.MicrophoneMuted)
                            ChangeMicrophoneMute();
                    });
                    break;
                case "end_channel":
                    ThreadManagerUtil.RunInUI(async () => {
                        await LeaveRoom();
                    });
                    break;
                case "remove_from_channel":
                default:
                    Debug.WriteLine(message);
                    break;
            }
        }
    }
}
