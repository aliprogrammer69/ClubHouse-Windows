using Prism.Mvvm;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public abstract class BaseViewModel : BindableBase {
        private string _title;
        public string Title {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
