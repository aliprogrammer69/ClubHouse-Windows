using ClubHouse.Domain.Services;
using ClubHouse.UI.DesktopApp.Handler;
using ClubHouse.UI.DesktopApp.Services;
using ClubHouse.UI.DesktopApp.ViewModels;
using ClubHouse.UI.DesktopApp.Views;

using Prism.Ioc;

using Unity;

namespace ClubHouse.UI.DesktopApp.Extensions {
    public static class ContainerRegisteryExtensions {
        public static void AddUI(this IContainerRegistry containerRegistry) {
            RegisterViewModels(containerRegistry);
            RegisterViews(containerRegistry);
            RegisterUtils(containerRegistry);
            RegisterDialogs(containerRegistry);
        }

        #region Private Methods
        private static void RegisterViews(IContainerRegistry containerRegistry) {
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<ErrorView, ErrorViewModel>();
            containerRegistry.RegisterForNavigation<RegisterView, RegisterViewModel>();
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
            containerRegistry.RegisterForNavigation<RoomView, RoomViewModel>();
            containerRegistry.RegisterForNavigation<EmptyView>();
            containerRegistry.RegisterForNavigation<ProfileView, ProfileViewModel>();
            containerRegistry.RegisterForNavigation<UserListView, UserListViewModel>();
            containerRegistry.RegisterForNavigation<InviteToChannelView, InviteToChannelViewModel>();
            containerRegistry.RegisterSingleton<NotificationView>(p => new NotificationView() {
                DataContext = p.Resolve<NotificationViewModel>()
            });
            containerRegistry.RegisterForNavigation<EventsView, EventsViewModel>();
            containerRegistry.Register<CreateRoomView>(p => new CreateRoomView() {
                DataContext = p.Resolve<CreateRoomViewModel>()
            });
            containerRegistry.Register<InvitePeopleView>(p => new InvitePeopleView() {
                DataContext = p.Resolve<InvitePeopleViewModel>()
            });
            containerRegistry.RegisterForNavigation<ClubView, ClubViewModel>();
            containerRegistry.Register<JoinAsSpeakerView>(p => new JoinAsSpeakerView() {
                DataContext = p.Resolve<JoinAsSpeakerViewModel>()
            });
        }

        private static void RegisterViewModels(IContainerRegistry containerRegistry) {
            containerRegistry.RegisterSingleton<NotificationViewModel>();
            containerRegistry.RegisterSingleton<EventsViewModel>();
            containerRegistry.Register<CreateRoomViewModel>();
            containerRegistry.Register<InvitePeopleViewModel>();
            containerRegistry.Register<JoinAsSpeakerViewModel>();
        }

        private static void RegisterUtils(IContainerRegistry containerRegistry) {
            containerRegistry.RegisterSingleton<ClubHouseBootStrapper>();
            containerRegistry.RegisterSingleton<RoomManagerService>()
                             .RegisterSingleton<IMessageService, EventAggregatorMessageService>();
        }

        private static void RegisterDialogs(IContainerRegistry containerRegistry) {
            //containerRegistry.RegisterDialog<FooDialog, FooViewModel>();
        }

        #endregion
    }
}
