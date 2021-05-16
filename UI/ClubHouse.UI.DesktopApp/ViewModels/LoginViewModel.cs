using ClubHouse.Common;
using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Extensions;
using ClubHouse.UI.DesktopApp.Views;

using Prism.Commands;
using Prism.Regions;

using System;
using System.Text.RegularExpressions;
using System.Timers;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class LoginViewModel : BaseViewModel {
        private readonly IAuthService _authService;
        private readonly IProfileService _profileService;
        private readonly IRegionManager _regionManager;
        private readonly IAccountService _accountService;
        private readonly IMessageService _messageService;
        private DateTime? _lastResend = null;
        public LoginViewModel(IAuthService authService,
                              IProfileService profileService,
                              IRegionManager regionManager,
                              IAccountService accountService,
                              IMessageService messageService) {
            _authService = authService;
            _profileService = profileService;
            _regionManager = regionManager;
            _accountService = accountService;
            _messageService = messageService;
            InitializeCommands();
            Timer resendTimer = new Timer(1_000);
            resendTimer.Elapsed += ResendTimer_Elapsed;
        }

        

        public string _phoneNumber;
        public string PhoneNumber {
            get => _phoneNumber;
            set {
                SetProperty(ref _phoneNumber, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public string _confirmCode;
        public string ConfirmCode {
            get => _confirmCode;
            set {
                SetProperty(ref _confirmCode, value);
                ConfirmCommand.RaiseCanExecuteChanged();
            }

        }

        public int _index = 0;
        public int Index { get => _index; set => SetProperty(ref _index, value); }

        private bool _showProgress;
        public bool ShowProgress {
            get => _showProgress;
            set {
                SetProperty(ref _showProgress, value);
                LoginCommand.RaiseCanExecuteChanged();
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        #region Commands

        public DelegateCommand LoginCommand { get; protected set; }
        public bool CanLogin() =>
            !string.IsNullOrEmpty(_phoneNumber) &&
            !ShowProgress &&
            Regex.IsMatch(_phoneNumber, @"^\s*\+?\s*([0-9][\s-]*){9,}$");


        public async void Login() {
            ShowProgress = true;
            _accountService.Reset();
            var response = await _authService.StartPhoneNumberAuth(_phoneNumber);
            if (response?.Success == true)
                Index = 1;
            else
                _messageService.Show($"Login failed. {response.Error_message}");
            _lastResend = DateTime.Now;
            ResendCommand.RaiseCanExecuteChanged();
            ShowProgress = false;
        }

        public DelegateCommand ConfirmCommand { get; protected set; }
        public bool CanConfirm() =>
            !string.IsNullOrEmpty(_confirmCode) &&
            !ShowProgress &&
            _confirmCode.Length >= 4 &&
            Regex.IsMatch(_confirmCode, "^\\d*$");


        public async void Confirm() {
            ShowProgress = true;
            var confirmResponse = await _authService.CompletePhoneNumberAuth(_phoneNumber, _confirmCode);
            ShowProgress = false;
            if (confirmResponse == null || !confirmResponse.Success) {
                _regionManager.ShowErrorView("Failed to confirm.");
                return;
            }

            if (string.IsNullOrEmpty(confirmResponse.Auth_token)) {
                _messageService.Show("Failed to confirm. incurrect confirm code.");
                return;
            }

            if (confirmResponse.Is_waitlisted) {
                _regionManager.ShowErrorView("You're still on the waitlist. Find your friends to get yourself in.");
                return;
            }

            if (confirmResponse.Is_onboarding) {
                _regionManager.RequestNavigate(Regions.MainRegionName, nameof(RegisterView));
                return;
            }

            GetProfileResponse userInfo = await _profileService.GetInfo();
            if (userInfo != null && !userInfo.Success) {
                _regionManager.ShowErrorView($"[!] Cannot get user information. Please try again. {userInfo?.Error_message}");
                return;
            }

            _regionManager.ShowMainView(userInfo);
        }

        public DelegateCommand ResendCommand { get; protected set; }
        private async void Resend() {
            await _authService.ResendPhoneNumberAuth(_phoneNumber);
            _lastResend = DateTime.Now;
            ResendCommand.RaiseCanExecuteChanged();
            _messageService.Show("confirm code sent.");
        }

        private bool CanResend() {
            return _lastResend.HasValue && (DateTime.Now - _lastResend.Value).TotalMinutes >= 1;
        }

        public DelegateCommand ChangeNumberCommand { get => new(ChangeNumber); }
        private void ChangeNumber() {
            Index = 0;
        }

        #endregion

        #region Private Methods
        private void InitializeCommands() {
            LoginCommand = new DelegateCommand(Login, CanLogin);
            ConfirmCommand = new DelegateCommand(Confirm, CanConfirm);
            ResendCommand = new DelegateCommand(Resend, CanResend);
        }

        private void ResendTimer_Elapsed(object sender, ElapsedEventArgs e) {
            ResendCommand.RaiseCanExecuteChanged();
        }
        #endregion
    }
}
