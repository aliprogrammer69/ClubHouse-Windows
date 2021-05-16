using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Events;

using Prism.Events;

namespace ClubHouse.UI.DesktopApp.Services {
    public class EventAggregatorMessageService : IMessageService {
        private readonly IEventAggregator _eventAggregator;

        public EventAggregatorMessageService(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;
        }

        public void Show(string message) {
            _eventAggregator.GetEvent<ShowMessageEvent>().Publish(message);
        }
    }
}
