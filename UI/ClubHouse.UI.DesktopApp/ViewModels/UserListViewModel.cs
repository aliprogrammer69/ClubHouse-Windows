using ClubHouse.Common;
using ClubHouse.Domain.Models;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Handler;
using ClubHouse.UI.DesktopApp.Models;
using ClubHouse.UI.DesktopApp.Views;

using Prism.Commands;
using Prism.Regions;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class UserListViewModel : NavigationAwareViewModel {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IRegionManager _regionManager;
        private UserListToken _token;
        public UserListViewModel(IUserService userService,
                                 IMessageService messageService,
                                 IRegionManager regionManager,
                                 RoomManagerService roomManagerService) : base(roomManagerService) {
            _userService = userService;
            _messageService = messageService;
            _regionManager = regionManager;
            InitializeCollection();
        }
        public ObservableCollection<BaseUserInfo> AllItems { get; protected set; }
        public ICollectionView UsersView { get; protected set; }

        private string _searchQuery;
        public string SearchQuery {
            get => _searchQuery; set {
                SetProperty(ref _searchQuery, value);
                if (string.IsNullOrEmpty(value))
                    SearchUser();
            }
        }

        private string _profileRegionName;
        public string ProfileRegionName { get => _profileRegionName; set => SetProperty(ref _profileRegionName, value); }

        public void OnScrollChanged(object sender, ScrollChangedEventArgs e) {
            if (e.VerticalChange > 0 &&
                e.ExtentHeight - (e.VerticalOffset + e.ViewportHeight) <= (e.ExtentHeight * 10) / 100) {
                _token.PageIndex++;
                InitializeItems();
                e.Handled = true;
            }
        }

        #region Commands
        public DelegateCommand<long?> ShowProfileCommand { get => new(ShowProfile); }
        private void ShowProfile(long? userId) {
            long id = 0;
            if (_regionManager.Regions[_profileRegionName]
                             .NavigationService.Journal.CurrentEntry?.Parameters.TryGetValue(Consts.ProfileUserIdNavigationParameterKey, out id) == true &&
                             userId == id)
                return;

            NavigationParameters parameter = new NavigationParameters();
            parameter.Add(Consts.ProfileUserIdNavigationParameterKey, userId);
            _regionManager.RequestNavigate(_profileRegionName, nameof(ProfileView), parameter);
        }

        public DelegateCommand SearchUserCommand { get => new DelegateCommand(SearchUser); }
        private void SearchUser() {
            UsersView.Refresh();
        }
        #endregion

        #region INavigationAware members
        public override bool IsNavigationTarget(NavigationContext navigationContext) {
            return navigationContext.Parameters.TryGetValue(Consts.UserListTokenParameterKey, out UserListToken token) && _token.Equals(token);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext) {
            base.OnNavigatedTo(navigationContext);

            if (_token != null)
                return;
            if (!navigationContext.Parameters.TryGetValue(Consts.UserListTokenParameterKey, out _token))
                _messageService.Show("Token Missed.");

            Random rnd = new Random();
            ProfileRegionName = $"{Regions.UserListProfileContainerRegionName}_{_token.UserId}_{_token.Action}_{rnd.Next()}";

            Title = $"{_token.Name}  {_token.Action}";

            InitializeItems();
        }

        #endregion

        #region Private Methods
        private bool MatchQuery(BaseUserInfo userInfo) {
            return string.IsNullOrEmpty(_searchQuery) || userInfo.Name.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase);
        }

        private void InitializeCollection() {
            AllItems = new ObservableCollection<BaseUserInfo>();
            CollectionViewSource usersSource = new CollectionViewSource() {
                Source = AllItems
            };
            UsersView = usersSource.View;
            UsersView.Filter = (u) => u is BaseUserInfo userInfo && MatchQuery(userInfo);
        }

        private void InitializeItems() {
            switch (_token.Action) {
                case UserLoadingActions.Followers:
                    GetFollowers(_token.UserId, _token.PageIndex, _token.PageSize);
                    break;
                case UserLoadingActions.Followings:
                    GetFollowings(_token.UserId, _token.PageIndex, _token.PageSize);
                    break;
                case UserLoadingActions.MutaulFollowers:
                    GetMutaulFollowers(_token.UserId, _token.PageIndex, _token.PageSize);
                    break;
            }
        }

        private void GetFollowers(long userId, int pageIndex = 1, int pageSize = 50) {
            var response = Task.Run(async () => await _userService.GetFollowers(new UserPagingRequest() {
                User_id = userId,
                Page = pageIndex,
                Page_size = pageSize
            })).Result;
            if (response?.Success != true) {
                _messageService.Show($"Failed to get followers. {response.Error_message}");
                return;
            }
            AllItems.AddRange(response.Users);
        }

        private void GetFollowings(long userId, int pageIndex = 1, int pageSize = 50) {
            var response = Task.Run(async () => await _userService.GetFollowing(new UserPagingRequest() {
                User_id = userId,
                Page = pageIndex,
                Page_size = pageSize
            })).Result;
            if (response?.Success != true) {
                _messageService.Show($"Failed to get followings. {response.Error_message}");
                return;
            }
            AllItems.AddRange(response.Users);
        }

        private void GetMutaulFollowers(long userId, int pageIndex = 1, int pageSize = 50) {
            //var response = Task.Run(async () => await _userService.GetMutualFollows(new UserPagingRequest() {
            //    User_id = userId,
            //    Page = pageIndex,
            //    Page_size = pageSize
            //})).Result;
            //if (response?.Success != true) {
            //    _messageService.Show($"Failed to get mutau follows. {response.Error_message}");
            //    return;
            //}
            //AllItems.AddRange(response.Users);
        }
        #endregion
    }
}
