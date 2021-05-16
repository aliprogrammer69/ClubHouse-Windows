using ClubHouse.Common;
using ClubHouse.Domain.Models;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Events;
using ClubHouse.UI.DesktopApp.Extensions;
using ClubHouse.UI.DesktopApp.Handler;
using ClubHouse.UI.DesktopApp.Views;

using MaterialDesignThemes.Wpf;

using Prism.Commands;
using Prism.Events;
using Prism.Regions;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

using Unity;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class MainViewModel : BaseViewModel, INavigationAware {
        private readonly IChannelService _channelService;
        private readonly IUserService _userService;
        private readonly IClubService _clubService;
        private readonly IProfileService _profileService;
        private readonly IAuthService _authService;
        private readonly IRegionManager _regionManager;
        private readonly RoomManagerService _roomManagerService;
        private readonly IMessageService _messageService;
        private readonly IUnityContainer _unityContainer;
        private ChannelModel selectedRoom;
        private GetProfileResponse _accountInfo;
        private string _searchKeyword;
        private SearchType searchType;
        private IEnumerable<BaseUserInfo> userSearchResult;
        private BaseUserInfo selectedUser;
        private IEnumerable<ClubModel> clubSearchResult;
        private ClubModel selectedClub;

        public MainViewModel(IChannelService channelService,
                             IRegionManager regionManager,
                             RoomManagerService roomManagerService,
                             IUserService userService,
                             IClubService clubService,
                             IProfileService profileService,
                             IAuthService authService,
                             IMessageService messageService,
                             IEventAggregator eventAggregator,
                             IUnityContainer unityContainer) {
            _channelService = channelService;
            _userService = userService;
            _clubService = clubService;
            _profileService = profileService;
            _authService = authService;
            _regionManager = regionManager;
            _roomManagerService = roomManagerService;
            _messageService = messageService;
            _unityContainer = unityContainer;
            RoomList = new ObservableCollection<ChannelModel>();
            var viewRoomListSource = new CollectionViewSource() {
                Source = RoomList
            };
            ViewRoomList = viewRoomListSource.View;
            ViewRoomList.Filter = r => r is ChannelModel c && (
                string.IsNullOrEmpty(SearchKeyword) ||
                (c.Topic?.ToLower().Contains(SearchKeyword) ?? false) ||
                (c.Club_name?.ToLower().Contains(SearchKeyword) ?? false) ||
                (c.Users.Any(u => u.Name?.ToLower().Contains(SearchKeyword) ?? false)));

            eventAggregator.GetEvent<ProfileUpdateEvent>().Subscribe(OnProfileUpdated);

            InitializeCommands();
        }

        #region Properties
        public ObservableCollection<ChannelModel> RoomList { get; set; }
        public ICollectionView ViewRoomList { get; set; }
        public ChannelModel SelectedRoom { get => selectedRoom; set => SelectRoom(value); }

        public GetProfileResponse AccountInfo { get => _accountInfo; set => SetProperty(ref _accountInfo, value); }

        public IEnumerable<BaseUserInfo> UserSearchResult { get => userSearchResult; set => SetProperty(ref userSearchResult, value); }
        public BaseUserInfo SelectedUser { get => selectedUser; set => ShowProfile(value.User_id); }

        public IEnumerable<ClubModel> ClubSearchResult { get => clubSearchResult; set => SetProperty(ref clubSearchResult, value); }
        public ClubModel SelectedClub { get => selectedClub; set => ShowClub(value); }

        public SearchType SearchType { get => searchType; set => SetProperty(ref searchType, value); }
        public bool IsSearching { get => !string.IsNullOrEmpty(SearchKeyword); }
        public string SearchKeyword {
            get => _searchKeyword; set {
                SetProperty(ref _searchKeyword, value);
                if (string.IsNullOrEmpty(_searchKeyword))
                    Search();
            }
        }

        private bool _showProfileSideBar;
        public bool ShowProfileSideBar { get => _showProfileSideBar; set => SetProperty(ref _showProfileSideBar, value); }

        public string Version { get => $"Version {APIConsts.API_BUILD_VERSION}"; }

        private bool _showSideBar;
        public bool ShowSideBar {
            get => _showSideBar;
            set => SetProperty(ref _showSideBar, value);
        }
        #endregion

        #region Commands
        public DelegateCommand RefreshCommand { get => new(RefreshList); }
        public DelegateCommand SelectClubSearchCommand { get => new(SelectClubSearch); }
        public DelegateCommand SelectRoomSearchCommand { get => new(SelectRoomSearch); }
        public DelegateCommand SelectPeopleSearchCommand { get => new(SelectPeopleSearch); }
        public DelegateCommand<long?> ShowProfileCommand { get => new(ShowProfile); }
        public DelegateCommand SearchCommand { get => new(Search); }
        public DelegateCommand ShowNotificationsCommand { get => new(ShowNotifications); }
        public DelegateCommand ShowEventsCommand { get => new(ShowEvents); }
        public DelegateCommand<ChannelModel> HideChannelCommand { get => new(HideChannel); }
        public DelegateCommand ShowCreateRoomCommand { get => new(ShowCreateRoom); }
        public DelegateCommand ShowInvitePeopleCommand { get; protected set; }
        public DelegateCommand<string> OpenExternalUrl { get => new(Utils.OpenExternalUrl); }
        public DelegateCommand LogoutCommand { get => new(Logout); }
        #endregion

        #region INavigationAware members
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;
        public void OnNavigatedFrom(NavigationContext navigationContext) {
        }

        public void OnNavigatedTo(NavigationContext navigationContext) {
            if (navigationContext.Parameters.ContainsKey(Consts.UserInfoNavigationParameterKey))
                AccountInfo = navigationContext.Parameters.GetValue<GetProfileResponse>(Consts.UserInfoNavigationParameterKey);
            RefreshList();
        }
        #endregion

        #region Methods

        public async void RefreshList() {
            var apiResult = await _channelService.Get();
            if (apiResult == null || !apiResult.Success) {
                _messageService.Show($"Failed to get room list. error message is {apiResult.Error_message}");
            }
            else if (apiResult.Channels != null) {
                RoomList.Clear();
                RoomList.AddRange(apiResult.Channels);
            }
            Search();
        }

        public async void SelectRoom(ChannelModel room) {
            if (string.IsNullOrEmpty(room?.Channel)) {
                return;
            }

            var joinResult = await _roomManagerService.JoinRoom(room.Channel);
            if (joinResult?.Success == true) {
                _regionManager.ShowRoom(joinResult);
                SetProperty(ref selectedRoom, room);
            }
        }

        public async void HideChannel(ChannelModel room) {
            var apiResult = await _channelService.Hide(room.Channel);
            if (!apiResult.Success) {
                _messageService.Show($"{apiResult.Error_message}");
                return;
            }

            RefreshList();
        }

        public void SelectClubSearch() {
            SearchType = SearchType.Club;
        }

        public void SelectPeopleSearch() {
            SearchType = SearchType.People;
        }

        public void SelectRoomSearch() {
            SearchType = SearchType.Room;
        }

        public async void Search() {
            RaisePropertyChanged(nameof(IsSearching));
            if (string.IsNullOrEmpty(_searchKeyword)) {
                ViewRoomList.Refresh();
                return;
            }

            switch (SearchType) {
                case SearchType.Room:
                    ViewRoomList.Refresh();
                    break;
                case SearchType.People:
                    if (SearchKeyword.Length < 3) {
                        UserSearchResult = Enumerable.Empty<BaseUserInfo>();
                    }
                    else {
                        var userSearchApiResult = await _userService.Search(new UserSearchRequest() {
                            Query = SearchKeyword
                        });
                        if (userSearchApiResult.Success && userSearchApiResult.Users != null) {
                            UserSearchResult = userSearchApiResult.Users;
                        }
                    }
                    break;
                case SearchType.Club:
                    if (SearchKeyword.Length < 3) {
                        ClubSearchResult = Enumerable.Empty<ClubModel>();
                    }
                    else {
                        var clubSearchApiResult = await _clubService.Search(new ClubSearchRequest() {
                            Query = SearchKeyword
                        });
                        if (clubSearchApiResult.Success && clubSearchApiResult.Clubs != null) {
                            ClubSearchResult = clubSearchApiResult.Clubs;
                        }
                    }
                    break;
            }
        }

        private void ShowProfile(long? userId) {
            long id = 0;
            if (_regionManager.Regions[Regions.RoomContainerRegionName]
                             .NavigationService.Journal.CurrentEntry?.Parameters.TryGetValue(Consts.ProfileUserIdNavigationParameterKey, out id) == true &&
                             id == userId.Value) {
                return;
            }

            NavigationParameters parameters = new();
            parameters.Add(Consts.ProfileUserIdNavigationParameterKey, userId ?? 0);
            _regionManager.RequestNavigate(Regions.RoomContainerRegionName, nameof(ProfileView), parameters);
            ShowSideBar = false;
        }

        private void ShowClub(ClubModel club) {
            if (club == null)
                return;

            long id = 0;
            if (_regionManager.Regions[Regions.RoomContainerRegionName]
                             .NavigationService.Journal.CurrentEntry?.Parameters.TryGetValue(Consts.ClubIdNavigationParameterKey, out id) == true &&
                             id == club.Club_id) {
                return;
            }

            NavigationParameters parameters = new();
            parameters.Add(Consts.ClubIdNavigationParameterKey, club.Club_id);
            _regionManager.RequestNavigate(Regions.RoomContainerRegionName, nameof(ClubView), parameters);
            SetProperty(ref selectedClub, club);
            ShowSideBar = false;
        }

        private void OnProfileUpdated(UserInfo userInfo) {
            AccountInfo.User_profile.Name = userInfo.Name;
            AccountInfo.User_profile.Username = userInfo.Username;
            AccountInfo.User_profile.Photo_url = userInfo.ThumbnailPhotoUrl;
            RaisePropertyChanged(nameof(AccountInfo));
        }

        private void ShowNotifications() {
            NotificationView view = _unityContainer.Resolve<NotificationView>();
            DialogHost.Show(view);
            ShowSideBar = false;
        }

        private void ShowEvents() {
            _regionManager.RequestNavigate(Regions.RoomContainerRegionName, nameof(EventsView));
            ShowSideBar = false;
        }

        private void ShowCreateRoom() {
            CreateRoomView view = _unityContainer.Resolve<CreateRoomView>();
            DialogHost.Show(view);
            ShowSideBar = false;
        }

        private async void ShowInvitePeople() {
            InvitePeopleView view = _unityContainer.Resolve<InvitePeopleView>();
            ShowSideBar = false;
            await DialogHost.Show(view);
            RefereshAccountInfo();
        }

        private bool CanShowInvitePeople() => _accountInfo.Num_invites > 0;

        private async void RefereshAccountInfo() {
            var apiResult = await _profileService.GetInfo();
            if (apiResult?.Success == true) {
                AccountInfo = apiResult;
            }
        }

        private void Logout() {
            _authService.Logout();
            _regionManager.ShowLoginView();
        }

        private void InitializeCommands() {
            ShowInvitePeopleCommand = new DelegateCommand(ShowInvitePeople, CanShowInvitePeople);
        }
        #endregion
    }
}
