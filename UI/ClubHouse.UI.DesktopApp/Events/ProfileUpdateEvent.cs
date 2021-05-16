using ClubHouse.Domain.Models;

using Prism.Events;

namespace ClubHouse.UI.DesktopApp.Events {
    public class ProfileUpdateEvent : PubSubEvent<UserInfo> {
    }
}
