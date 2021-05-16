using ClubHouse.Domain.Models.Response;
using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Extensions;

using Prism.Commands;
using Prism.Regions;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class RegisterViewModel : BaseViewModel, INavigationAware {
        private readonly IProfileService _profileService;
        private readonly IMessageService _messageService;
        private readonly IRegionManager _regionManager;

        public RegisterViewModel(IMessageService messageService) {
            _messageService = messageService;
        }

        public RegisterViewModel(IProfileService profileService,
                                 IMessageService messageService,
                                 IRegionManager regionManager) {
            _profileService = profileService;
            _messageService = messageService;
            _regionManager = regionManager;
            InitializeCommand();
        }

        #region Props
        private string _firstName;
        public string FirstName {
            get => _firstName; set {
                SetProperty(ref _firstName, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        private string _lastName;
        public string LastName {
            get => _lastName; set {
                SetProperty(ref _lastName, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        private string _userName;
        public string UserName {
            get => _userName; set {
                SetProperty(ref _userName, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _inprogress;
        public bool Inprogress { get => _inprogress; set => SetProperty(ref _inprogress, value); }
        #endregion
        public DelegateCommand RegisterCommand { get; protected set; }

        #region INavigationAware Memebers
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) {
        }

        public void OnNavigatedTo(NavigationContext navigationContext) {
        }
        #endregion

        #region Private Methods
        private async void Register() {
            Inprogress = true;
            GetProfileResponse userInfo = await _profileService.GetInfo();
            BaseResponse response = null;
            if (string.IsNullOrEmpty(userInfo?.User_profile.Username)) {
                response = await _profileService.UpdateUsername(_userName);
                if (response?.Success != true) {
                    _messageService.Show($"Register failed. can't apply username. change username and try again {response?.Error_message}");
                    Inprogress = false;
                    return;
                }
            }

            if (string.IsNullOrEmpty(userInfo?.User_profile.Name)) {
                string name = string.Concat(_firstName, ' ', _lastName);
                response = await _profileService.UpdateName(name);
                if (response?.Success != true) {
                    _messageService.Show($"Register failed. can't apply name. {response?.Error_message}");
                    Inprogress = false;
                    return;
                }
            }

            userInfo = await _profileService.GetInfo();
            if (userInfo != null && !userInfo.Success) {
                Inprogress = false;
                _regionManager.ShowErrorView($"[!] Cannot get user information. please try again. {userInfo?.Error_message}");
                return;
            }

            Inprogress = false;
            _regionManager.ShowMainView(userInfo);
        }

        private bool CanRegister() =>
            !Inprogress &&
            !string.IsNullOrEmpty(_firstName) &&
            !string.IsNullOrEmpty(_lastName) &&
            !string.IsNullOrEmpty(_userName);

        private void InitializeCommand() {
            RegisterCommand = new DelegateCommand(Register, CanRegister);
        }
        #endregion
    }
}
