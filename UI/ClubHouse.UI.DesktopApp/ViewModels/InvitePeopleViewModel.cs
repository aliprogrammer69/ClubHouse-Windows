using ClubHouse.Domain.Services;
using MaterialDesignThemes.Wpf;
using Prism.Commands;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class InvitePeopleViewModel : BaseViewModel {
        private readonly IProfileService _profileService;
        private readonly IMessageService _messageService;
        private string phoneNumber;
        private string name;

        public InvitePeopleViewModel(IProfileService profileService, IMessageService messageService) {
            _profileService = profileService;
            _messageService = messageService;
        }

        public string Name { get => name; set => SetProperty(ref name, value); }
        public string PhoneNumber { get => phoneNumber; set => SetProperty(ref phoneNumber, value); }

        public DelegateCommand InviteCommand { get => new(Invite); }

        public async void Invite() {
            var apiResult = await _profileService.InviteToApp(Name, PhoneNumber);
            if (apiResult?.Success == true) {
                _messageService.Show($"{Name} has been invited to app");
                DialogHost.Close(null, null);
            }
            else {
                _messageService.Show(apiResult?.Error_message ?? "Error in invite people to app");
            }
        }
    }
}
