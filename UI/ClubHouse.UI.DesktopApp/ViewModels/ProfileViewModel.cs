
using ClubHouse.Common;
using ClubHouse.Domain.Models;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Events;
using ClubHouse.UI.DesktopApp.Extensions;
using ClubHouse.UI.DesktopApp.Handler;
using ClubHouse.UI.DesktopApp.Models;
using ClubHouse.UI.DesktopApp.Views;

using MaterialDesignThemes.Wpf;

using Microsoft.Win32;

using Prism.Commands;
using Prism.Events;
using Prism.Regions;

using System.Linq;
using System.Text;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class ProfileViewModel : NavigationAwareViewModel {
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IMessageService _messageService;
        private readonly IProfileService _profileService;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public ProfileViewModel(IUserService userService,
                                IAccountService accountService,
                                IMessageService messageService,
                                IProfileService profileService,
                                IRegionManager regionManager,
                                IEventAggregator eventAggregator,
                                RoomManagerService roomManagerService) : base(roomManagerService) {
            _userService = userService;
            _accountService = accountService;
            _messageService = messageService;
            _profileService = profileService;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
        }

        #region Props
        private UserInfo _userInfo;
        public UserInfo UserInfo { get => _userInfo; set => SetProperty(ref _userInfo, value); }

        private string _memberOfString;
        public string MemberofString { get => _memberOfString; set => SetProperty(ref _memberOfString, value); }

        private bool _following;
        public bool Following { get => _following; set => SetProperty(ref _following, value); }

        private bool _isOwnedProfile;
        public bool IsOwnedProfile { get => _isOwnedProfile; set => SetProperty(ref _isOwnedProfile, value); }
        #endregion

        #region INavigationAware Members

        public override bool IsNavigationTarget(NavigationContext navigationContext) {
            return _userInfo != null &&
                   navigationContext.Parameters.TryGetValue(Consts.ProfileUserIdNavigationParameterKey, out long userId) &&
                   userId == _userInfo.User_id;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext) {
            base.OnNavigatedTo(navigationContext);

            if (_userInfo != null)
                return;

            if (!navigationContext.Parameters.TryGetValue(Consts.ProfileUserIdNavigationParameterKey, out long userId)) {
                _messageService.Show($"{Consts.ProfileUserIdNavigationParameterKey} parameter missing.");
                return;
            }

            Initialize(userId);
        }

        #endregion

        #region Commands
        public DelegateCommand CloseCommand { get => new(Close); }
        public DelegateCommand<long?> ShowProfileCommand { get => new(ShowProfile); }
        public DelegateCommand FollowCommand { get => new(Follow); }
        public DelegateCommand UnFollowCommand { get => new(UnFollow); }
        public DelegateCommand<string> OpenExternalUrlCommand { get => new(Utils.OpenExternalUrl); }
        public DelegateCommand ShowFollowersCommand { get => new(ShowFollowers); }
        public DelegateCommand ShowFollowingsCommand { get => new(ShowFollowings); }
        public DelegateCommand ShowMutualFollowingsCommand { get => new(ShowMutualFollowers); }
        public DelegateCommand<string> UpdateUserNameCommand { get => new(UpdateUserName); }
        public DelegateCommand<string> UpdateNameCommand { get => new(UpdateName); }
        public DelegateCommand UpdateProfilePhotoCommand { get => new(UpdateProfilePhoto); }
        public DelegateCommand<string> UpdateBioCommand { get => new(UpdateBio); }
        public DelegateCommand<object> ShowEditDialogCommand { get => new(ShowEditDialog); }
        private void Close() {
            base.NavigationService.Close();
        }

        private void ShowEditDialog(object view) {
            DialogHost.Show(view);
        }

        private void ShowProfile(long? userId) {
            NavigationParameters parameters = new();
            parameters.Add(Consts.ProfileUserIdNavigationParameterKey, userId ?? 0);
            _regionManager.RequestNavigate(base.NavigationService.Region.Name, nameof(ProfileView), parameters);
        }

        private async void Follow() {
            var response = await _userService.Follow(_userInfo.User_id);
            if (!response.Success) {
                _messageService.Show($"Failed to follow. {response.Error_message}");
                return;
            }
            Following = true;
            RaisePropertyChanged();
        }

        private async void UnFollow() {
            var response = await _userService.Unfollow(_userInfo.User_id);
            if (response?.Success != true) {
                _messageService.Show($"Failed to unfollow. {response.Error_message}");
                return;
            }
            Following = false;
            RaisePropertyChanged();
        }

        private void ShowFollowers() {
            ShowUserList(UserLoadingActions.Followers);
        }

        private void ShowFollowings() {
            ShowUserList(UserLoadingActions.Followings);
        }

        private void ShowMutualFollowers() {
            ShowUserList(UserLoadingActions.MutaulFollowers);
        }

        private void ShowUserList(UserLoadingActions action) {
            NavigationParameters parameters = new();
            parameters.Add(Consts.UserListTokenParameterKey, new UserListToken() {
                Action = action,
                UserId = _userInfo.User_id,
                Name = _userInfo.Name
            });
            _regionManager.RequestNavigate(Regions.RoomContainerRegionName, nameof(UserListView), parameters);
        }

        private async void UpdateUserName(string username) {
            if (string.IsNullOrEmpty(username)) {
                _messageService.Show("Username cann't be empty");
                return;
            }

            var response = await _profileService.UpdateUsername(username);
            if (response?.Success == true) {
                UserInfo.Username = username;
                RaisePropertyChanged(nameof(UserInfo));
                _eventAggregator.GetEvent<ProfileUpdateEvent>().Publish(UserInfo);
            }
            else
                _messageService.Show($"Filed to update usename. {response?.Error_message}");
            MaterialDesignThemes.Wpf.DialogHost.Close(null, null);
        }

        private async void UpdateName(string name) {
            if (string.IsNullOrEmpty(name)) {
                _messageService.Show("Name cann't be empty");
                return;
            }

            var response = await _profileService.UpdateName(name);
            if (response?.Success == true) {
                UserInfo.Name = name;
                RaisePropertyChanged(nameof(UserInfo));
                _eventAggregator.GetEvent<ProfileUpdateEvent>().Publish(UserInfo);
            }
            else
                _messageService.Show($"Failed to update name. {response?.Error_message}");
            MaterialDesignThemes.Wpf.DialogHost.Close(null, null);
        }

        private async void UpdateProfilePhoto() {
            OpenFileDialog openFile = new();
            openFile.Filter = "Jpeg Files|*.jpg";
            if (openFile.ShowDialog() == true) {
                UpdateProfilePhotoResponse response = await _profileService.UpdatePhoto(openFile.FileName);
                if (response?.Success != true) {
                    _messageService.Show($"Failed to update photo. {response.Error_message}");
                    return;
                }
                UserInfo.Photo_url = response.Photo_Url.Replace("_thumbnail_250x250", null);
                RaisePropertyChanged(nameof(UserInfo));
                _eventAggregator.GetEvent<ProfileUpdateEvent>().Publish(UserInfo);
            }
        }

        private async void UpdateBio(string bio) {
            var response = await _profileService.UpdateBio(bio);
            if (response?.Success == true) {
                UserInfo.Bio = bio;
                RaisePropertyChanged(nameof(UserInfo));
            }
            else
                _messageService.Show($"Failed to update bio. {response?.Error_message}");
            MaterialDesignThemes.Wpf.DialogHost.Close(null, null);
        }
        #endregion

        #region Private Methods
        private async void Initialize(long userId) {
            var userResponse = await _userService.Get(userId);//TODO: Error. Check null reference error
            if (!userResponse.Success) {
                _messageService.Show($"Failed to get user info. error message is {userResponse.Error_message}");
                return;
            }
            UserInfo = userResponse.User_profile;

            MemberofString = GenerateMemberOfString();
            Following = _userInfo.Following;
            IsOwnedProfile = _accountService.CurrentConfig.UserId == _userInfo.User_id;
            RaisePropertyChanged();
        }

        private string GenerateMemberOfString() {
            if (_userInfo.Clubs?.Any() != true)
                return null;

            StringBuilder builder = new();
            builder.Append($"{_userInfo.Clubs.Count()} Clubs. ");
            builder.Append(string.Join(',', _userInfo.Clubs.Take(5).Select(c => $" {c.Name}")));

            return builder.ToString();
        }
        #endregion
    }
}
