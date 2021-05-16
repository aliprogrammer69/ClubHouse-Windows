
using ClubHouse.Common;
using ClubHouse.Domain.Models.Response;
using ClubHouse.UI.DesktopApp.Handler;
using ClubHouse.UI.DesktopApp.Models;
using ClubHouse.UI.DesktopApp.Views;

using MaterialDesignThemes.Wpf;

using Microsoft.Win32;

using Prism.Commands;
using Prism.Events;
using Prism.Regions;

using System;
using System.Windows;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class RoomViewModel : BaseViewModel, INavigationAware {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public RoomViewModel(RoomManagerService roomManagerService,
                             IRegionManager regionManager,
                             IEventAggregator eventAggregator,
                             InviteToChannelViewModel inviteToChannelViewModel) {
            Room = roomManagerService;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            InviteToChannelViewModel = inviteToChannelViewModel;
            InitializeCommands();

            Room.OnJoined -= Room_OnJoined;
            Room.OnJoined += Room_OnJoined;
            Room.OnLeave -= Room_OnJoined;
            Room.OnLeave += Room_OnJoined;
        }

        #region Props
        public RoomManagerService Room { get; set; }
        public InviteToChannelViewModel InviteToChannelViewModel { get; set; }

        private string _profileContainerRegionName;
        public string ProfileContainerRegionName {
            get => _profileContainerRegionName;
            set => SetProperty(ref _profileContainerRegionName, value);
        }

        #endregion

        #region Commands
        public DelegateCommand ChangeRaiseHandCommand { get; protected set; }
        public DelegateCommand LeaveRoomCommand { get; protected set; }
        public DelegateCommand CopyRoomUrlCommand { get; protected set; }
        public DelegateCommand ChangeSpeakerMuteCommand { get; protected set; }
        public DelegateCommand ChangeMicrophoneMuteCommand { get; protected set; }
        public DelegateCommand GetRoomDetailCommand { get; protected set; }
        public DelegateCommand InviteUserCommand { get; protected set; }

        public DelegateCommand SearchUserCommand { get => new(Room.JoinedUsers.Search); }
        public DelegateCommand<long?> ShowProfileCommand { get => new(ShowProfile); }
        public DelegateCommand<BindableChannelUser> InviteUserToSpeakCommand { get => new(Room.InviteUserToSpeak); }
        public DelegateCommand<BindableChannelUser> MakeUserModeratorCommand { get => new(Room.MakeUserModerator); }
        public DelegateCommand<BindableChannelUser> MoveToAudianceCommand { get => new(Room.MoveToAudiance); }
        public DelegateCommand<BindableChannelUser> MuteUserMicrophoneCommand { get => new(Room.ChangeUserMicrophoneMuted); }
        public DelegateCommand<BindableChannelUser> AcceptRaiseHandCommand { get => new(Room.AcceptRaiseHand); }
        public DelegateCommand<BindableChannelUser> RejectRaiseHandCommand { get => new(Room.RejectRaiseHand); }


        private void ChangeRaiseHand() {
            Room.ChangeRaiseHand();
        }

        private async void LeaveRoom() {
            if (await Room.LeaveRoom()) {
                _regionManager.RequestNavigate(Regions.RoomContainerRegionName, nameof(EmptyView));
            }
        }

        private void CopyRoomUrl() {
            if (!string.IsNullOrEmpty(Room.ChannelInfo?.Url))
                Clipboard.SetText(Room.ChannelInfo.Url);
        }

        private void ShowProfile(long? userId) {
            long id = 0;
            if (_regionManager.Regions[_profileContainerRegionName]
                             .NavigationService.Journal.CurrentEntry?.Parameters.TryGetValue(Consts.ProfileUserIdNavigationParameterKey, out id) == true
                             && id == userId.Value) {
                return;
            }
            NavigationParameters parameter = new() {
                { Consts.ProfileUserIdNavigationParameterKey, userId }
            };
            _regionManager.RequestNavigate(_profileContainerRegionName, nameof(ProfileView), parameter);
        }

        private bool CanActionButtonExecute() {
            return Room.Joined;
        }

        #endregion

        #region INavigationAware Members
        public void OnNavigatedFrom(NavigationContext navigationContext) {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext) {
            var apiResponse = navigationContext.Parameters.GetValue<InitChannelResponse>(Consts.ChannelResponseNavigationParameterKey);
            return apiResponse?.Channel == Room.ChannelInfo?.Channel;
        }

        public void OnNavigatedTo(NavigationContext navigationContext) {
            var apiResponse = navigationContext.Parameters.GetValue<InitChannelResponse>(Consts.ChannelResponseNavigationParameterKey);
            if (apiResponse != null) {
                if (Room.ChannelInfo == null || apiResponse.Channel != Room.ChannelInfo.Channel) {
                    Room.InitRoom(apiResponse);
                    Random rnd = new();
                    _profileContainerRegionName = string.Concat(Regions.RoomViewProfileContainerRegionName, "_", apiResponse.Channel_id, "_", rnd.Next());
                }
            }
        }

        #endregion

        #region Private Methods
        private void InitializeCommands() {
            ChangeRaiseHandCommand = new DelegateCommand(ChangeRaiseHand, CanActionButtonExecute);
            LeaveRoomCommand = new DelegateCommand(LeaveRoom, CanActionButtonExecute);
            CopyRoomUrlCommand = new DelegateCommand(CopyRoomUrl, CanActionButtonExecute);
            ChangeSpeakerMuteCommand = new DelegateCommand(Room.ChangeSpeakerMute, CanActionButtonExecute);
            ChangeMicrophoneMuteCommand = new DelegateCommand(Room.ChangeMicrophoneMute, CanActionButtonExecute);
            GetRoomDetailCommand = new DelegateCommand(Room.GetDetail, CanActionButtonExecute);
            InviteUserCommand = new DelegateCommand(OpenJoinUserDialog, CanActionButtonExecute);
        }

        private void Room_OnJoined(BaseChannelResponse channel) {
            ChangeRaiseHandCommand.RaiseCanExecuteChanged();
            LeaveRoomCommand.RaiseCanExecuteChanged();
            CopyRoomUrlCommand.RaiseCanExecuteChanged();
            ChangeSpeakerMuteCommand.RaiseCanExecuteChanged();
            ChangeMicrophoneMuteCommand.RaiseCanExecuteChanged();
            GetRoomDetailCommand.RaiseCanExecuteChanged();
            InviteUserCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged();
        }

        private async void OpenJoinUserDialog() {
            await InviteToChannelViewModel.Initialize();
            var view = new InviteToChannelView() {
                DataContext = InviteToChannelViewModel
            };
            await DialogHost.Show(view);
        }
        #endregion
    }
}
