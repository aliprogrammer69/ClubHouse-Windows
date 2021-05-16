using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubHouse.Domain.Models;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Handler;
using Prism.Commands;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class InviteToChannelViewModel : BaseViewModel {
        private readonly RoomManagerService _roomManagerService;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IChannelService _channelService;
        private readonly IMessageService _messageService;
        private string searchQuery;
        private IEnumerable<BaseUserInfo> userSearchResult;

        public InviteToChannelViewModel(RoomManagerService roomManagerService,
                                        IUserService userService,
                                        IAccountService accountService,
                                        IChannelService channelService,
                                        IMessageService messageService) {
            _roomManagerService = roomManagerService;
            _userService = userService;
            _accountService = accountService;
            _channelService = channelService;
            _messageService = messageService;
        }

        public string SearchQuery {
            get => searchQuery;
            set {
                SetProperty(ref searchQuery, value);
                Search();
            }
        }

        private IEnumerable<BaseUserInfo> baseResult;

        public IEnumerable<BaseUserInfo> UserSearchResult { get => userSearchResult; set => SetProperty(ref userSearchResult, value); }

        public DelegateCommand<BaseUserInfo> InviteToJoinCommand { get => new(Invite); }

        public async Task<bool> Initialize() {
            SearchQuery = null;
            var userId = _accountService.CurrentConfig?.UserId;
            if (userId == null)
                return false;

            var apiResult = await _userService.GetFollowers(new UserPagingRequest() {
                User_id = userId.Value
            });
            baseResult = apiResult?.Users;
            Search();
            return true;
        }

        public async void Search() {
            if (string.IsNullOrEmpty(SearchQuery)) {
                UserSearchResult = baseResult;
            }
            else if (SearchQuery.Length < 3) {
                UserSearchResult = Enumerable.Empty<BaseUserInfo>();
            }
            else {
                var apiResult = await _userService.Search(new UserSearchRequest() {
                    Query = SearchQuery,
                    Followers_only = true
                });
                UserSearchResult = apiResult?.Users;
            }
        }

        public async void Invite(BaseUserInfo userInfo) {
            if (userInfo == null || string.IsNullOrEmpty(_roomManagerService.ChannelInfo?.Channel))
                return;

            var apiResult = await _channelService.InviteToJoin(new ChannelUserRequest() {
                Channel = _roomManagerService.ChannelInfo.Channel,
                User_id = userInfo.User_id
            });
            if (apiResult.Success) {
                _messageService.Show($"User {userInfo.Name} pinned");
            }
            else {
                _messageService.Show($"{apiResult.Error_message}");
            }
        }
    }
}
