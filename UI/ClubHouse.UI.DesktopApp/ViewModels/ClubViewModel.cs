using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubHouse.Common;
using ClubHouse.Domain.Models;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Extensions;
using Prism.Commands;
using Prism.Regions;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class ClubViewModel : BaseViewModel, INavigationAware {
        private readonly IClubService _clubService;
        private readonly IMessageService _messageService;
        private IRegionNavigationService _navigationService;
        private GetClubResponse clubInfo;
        private bool isFollower;

        public ClubViewModel(IClubService clubService, IMessageService messageService, IRegionNavigationService navigationService) {
            _clubService = clubService;
            _messageService = messageService;
            _navigationService = navigationService;

            InitializeCommands();
        }

        public GetClubResponse ClubInfo { get => clubInfo; set => SetProperty(ref clubInfo, value); }
        public bool IsFollower { 
            get => isFollower; 
            set {
                SetProperty(ref isFollower, value);
                if (ClubInfo != null)
                    ClubInfo.Is_follower = value;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) {
            return ClubInfo?.Club != null &&
                navigationContext.Parameters.TryGetValue(Consts.ClubIdNavigationParameterKey, out long clubId) &&
                clubId == ClubInfo.Club.Club_id;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) {

        }

        public void OnNavigatedTo(NavigationContext navigationContext) {
            _navigationService = navigationContext.NavigationService;
            ForwardCommand.RaiseCanExecuteChanged();
            BackCommand.RaiseCanExecuteChanged();

            if (ClubInfo != null)
                return;

            if (!navigationContext.Parameters.TryGetValue(Consts.ClubIdNavigationParameterKey, out long clubId)) {
                _messageService.Show($"{Consts.ClubIdNavigationParameterKey} parameter missing.");
                return;
            }

            Initialize(clubId);
        }

        public DelegateCommand CloseCommand { get => new(Close); }
        public DelegateCommand BackCommand { get; protected set; }
        public DelegateCommand ForwardCommand { get; protected set; }
        public DelegateCommand FollowCommand { get => new(Follow); }
        public DelegateCommand UnfollowCommand { get => new(Unfollow); }


        private void InitializeCommands() {
            BackCommand = new DelegateCommand(Back, CanBack);
            ForwardCommand = new DelegateCommand(Forward, CanForward);
        }

        private void Close() {
            _navigationService.Close();
        }

        private void Back() {
            _navigationService.Journal.GoBack();
            BackCommand.RaiseCanExecuteChanged();
            ForwardCommand.RaiseCanExecuteChanged();
        }

        private bool CanBack() => _navigationService.Journal.CanGoBack;


        private void Forward() {
            _navigationService.Journal.GoForward();
            BackCommand.RaiseCanExecuteChanged();
            ForwardCommand.RaiseCanExecuteChanged();
        }

        private bool CanForward() => _navigationService.Journal.CanGoForward;

        private async void Initialize(long clubId) {
            var apiResult = await _clubService.Get(clubId);
            if (apiResult?.Success != true) {
                _messageService.Show(apiResult?.Error_message ?? $"Failed to get club info");
                return;
            }
            ClubInfo = apiResult;
            IsFollower = ClubInfo.Is_follower;
        }

        private async void Follow() {
            if (ClubInfo?.Club == null)
                return;

            var apiResult = await _clubService.Follow(ClubInfo.Club.Club_id);
            if (apiResult?.Success != true) {
                _messageService.Show(apiResult?.Error_message ?? "Fail to follow club");
            }

            IsFollower = true;
            RaisePropertyChanged();
        }

        private async void Unfollow() {
            if (ClubInfo?.Club == null)
                return;

            var apiResult = await _clubService.Unfollow(ClubInfo.Club.Club_id);
            if (apiResult?.Success != true) {
                _messageService.Show(apiResult?.Error_message ?? "Fail to follow club");
            }

            IsFollower = false;
            RaisePropertyChanged();
        }

    }
}
