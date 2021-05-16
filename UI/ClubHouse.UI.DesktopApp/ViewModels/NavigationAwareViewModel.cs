using ClubHouse.UI.DesktopApp.Handler;

using Prism.Commands;
using Prism.Regions;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public abstract class NavigationAwareViewModel : BaseViewModel, INavigationAware {
        private readonly RoomManagerService _roomManagerService;
        protected IRegionNavigationService NavigationService;

        public NavigationAwareViewModel(RoomManagerService roomManagerService) {
            _roomManagerService = roomManagerService;
            InitializeCommands();
        }

        #region INavigationAware Members
        public abstract bool IsNavigationTarget(NavigationContext navigationContext);

        public virtual void OnNavigatedFrom(NavigationContext navigationContext) {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext) {
            NavigationService = navigationContext.NavigationService;
            ForceCommandsCanExecuteChanges();
        }
        #endregion

        #region Commands
        public DelegateCommand BackCommand { get; protected set; }
        private void Back() {
            NavigationService.Journal.GoBack();
            ForceCommandsCanExecuteChanges();
        }
        private bool CanBack() => NavigationService.Journal.CanGoBack;

        public DelegateCommand ForwardCommand { get; protected set; }
        private void Forward() {
            NavigationService.Journal.GoForward();
            ForceCommandsCanExecuteChanges();
        }
        private bool CanForward() => NavigationService.Journal.CanGoForward;
        #endregion

        #region Private Methods
        private void InitializeCommands() {
            BackCommand = new DelegateCommand(Back, CanBack);
            ForwardCommand = new DelegateCommand(Forward, CanForward);
        }

        private void ForceCommandsCanExecuteChanges() {
            BackCommand.RaiseCanExecuteChanged();
            ForwardCommand.RaiseCanExecuteChanged();
        }
        #endregion

    }
}

