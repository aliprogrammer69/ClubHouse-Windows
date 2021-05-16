using ClubHouse.UI.DesktopApp.Events;

using MaterialDesignThemes.Wpf;

using Prism.Events;

using System;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class MainWindowViewModel : BaseViewModel {
        public MainWindowViewModel(IEventAggregator eventAggregator) {
            Title = "ClubHouse For Windows - unofficial";
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
            MessageQueue.DiscardDuplicates = true;
            eventAggregator.GetEvent<ShowMessageEvent>().Subscribe(message => MessageQueue.Enqueue(message, "OK", null));
        }

        public SnackbarMessageQueue MessageQueue { get; protected set; }
    }
}
