using ClubHouse.Common;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Extensions;
using ClubHouse.UI.DesktopApp.Views;

using Prism.Regions;
using Prism.Services.Dialogs;

using System;
using System.Threading.Tasks;
using System.Windows;

namespace ClubHouse.UI.DesktopApp {

    public class ClubHouseBootStrapper {
        private readonly IAccountService _accountService;
        private readonly IProfileService _profileService;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService dialogService;

        public ClubHouseBootStrapper(IAccountService accountService,
                                     IProfileService profileService,
                                     IRegionManager regionManager,
                                     IDialogService dialogService) {
            _accountService = accountService;
            _profileService = profileService;
            _regionManager = regionManager;
            this.dialogService = dialogService;
        }

        [STAThread]
        public void Run() {
            if (!_accountService.Authenticated) {
                _regionManager.ShowLoginView();
                return;
            }
            Initialize();
        }

        #region Private Methods
        private void Initialize() {
            var waitListReponse = Task.Run(async () => await _profileService.CheckWaitlistStatus()).Result;
            if(waitListReponse  == null || !waitListReponse.Success) {
                _regionManager.ShowErrorView($"An error accoured while checking account info. please try again later. {waitListReponse.Error_message}");
                return;
            }

            if (waitListReponse.Is_waitlisted) {
                _regionManager.ShowErrorView("You're still on the waitlist. Find your friends to get yourself in.");
                return;
            }

            GetProfileResponse userInfo = Task.Run(async () => await _profileService.GetInfo()).Result;
            if (userInfo != null && !userInfo.Success) {
                _regionManager.ShowErrorView($"[!] Cannot get user information ({userInfo?.Error_message})");
                return;
            }

            if (string.IsNullOrEmpty(userInfo?.User_profile?.Username)) {
                _regionManager.RequestNavigate(Regions.MainRegionName, nameof(RegisterView));
                return;
            }

            _regionManager.ShowMainView(userInfo);
        }
        #endregion

    }
}
