using ClubHouse.Common;
using ClubHouse.Domain.Models;
using ClubHouse.Domain.Models.Request;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Handler;
using ClubHouse.UI.DesktopApp.Views;

using MaterialDesignThemes.Wpf;

using Prism.Commands;
using Prism.Regions;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class NotificationViewModel : BaseViewModel {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IRegionManager _regionManager;
        private readonly RoomManagerService _roomManagerService;
        private long _latestNnotificationId;
        private PagingRequest _pagingRequest = new PagingRequest();

        public NotificationViewModel(IUserService userService,
                                     IMessageService messageService,
                                     IRegionManager regionManager,
                                     RoomManagerService roomManagerService) {
            _userService = userService;
            _messageService = messageService;
            _regionManager = regionManager;
            _roomManagerService = roomManagerService;
            InitializeCollection();
            Getnotifications();
        }

        public ObservableCollection<NotificationModel> AllNotifications { get; protected set; }
        public ICollectionView NotificationsView { get; protected set; }
        private NotificationModel _selectedNotification;
        public NotificationModel SelectedNotification {
            get => _selectedNotification;
            set {
                SetProperty(ref _selectedNotification, value);
                ProccessSelectedNotification();
            }
        }

        private bool _fetchingNotifications;
        public bool FetchingNotifications { get => _fetchingNotifications; set => SetProperty(ref _fetchingNotifications, value); }

        #region Commands
        public DelegateCommand<long?> ShowProfileCommand { get => new(ShowProfile); }
        public DelegateCommand FetchNextPageCommand { get => new(FetchNextPage); }
        #endregion

        #region Private Methods

        private void InitializeCollection() {
            AllNotifications = new ObservableCollection<NotificationModel>();
            NotificationsView = CollectionViewSource.GetDefaultView(AllNotifications);
            NotificationsView.SortDescriptions.Add(new SortDescription(nameof(NotificationModel.Notification_Id), ListSortDirection.Descending));
        }

        private async void Getnotifications() {
            FetchingNotifications = true;
            NotificationsResponse notifications = await _userService.GetNotifications(_pagingRequest);
            if (!notifications.Success) {
                _messageService.Show($"Failed to notifications. {notifications.Error_message}");
                return;
            }

            IEnumerable<NotificationModel> newNotifications = notifications.Notifications.Where(n => n.Notification_Id > _latestNnotificationId);
            if (newNotifications.Any()) {
                AllNotifications.AddRange(newNotifications);
                NotificationsView.Refresh();
                _latestNnotificationId = newNotifications.Max(n => n.Notification_Id);
            }

            FetchingNotifications = false;
            _pagingRequest.TotalCount = notifications.Count;
            _pagingRequest.NextPageCount = notifications.Next;
            _pagingRequest.PreviousPageCount = notifications.Previous;
        }

        private void FetchNextPage() {
            if (_pagingRequest.NextPageCount > 0) {
                _pagingRequest.Page++;
                Getnotifications();
            }
        }

        private void ShowProfile(long? userId) {
            _regionManager.RequestNavigate(Regions.RoomContainerRegionName, nameof(ProfileView), new NavigationParameters() {
                        {Consts.ProfileUserIdNavigationParameterKey,userId }});
            DialogHost.Close(null, null);
        }

        private async void ProccessSelectedNotification() {
            switch (_selectedNotification.Type) {
                case Domain.NotificationType.Followed:
                    ShowProfile(_selectedNotification.User_profile.User_id);
                    break;
                case Domain.NotificationType.InviteToJoin:
                    var joinResult = await _roomManagerService.JoinRoom(_selectedNotification.Channel);
                    if (joinResult?.Success == true) {
                        _regionManager.RequestNavigate(Regions.RoomContainerRegionName, nameof(RoomView), new NavigationParameters() {
                            {Consts.ChannelResponseNavigationParameterKey,joinResult }
                        });
                        DialogHost.Close(null, null);
                    }
                    break;
                case Domain.NotificationType.SystemNotification:
                    break;
                case Domain.NotificationType.EventSchedule:
                    break;

            }
        }
        #endregion
    }
}
