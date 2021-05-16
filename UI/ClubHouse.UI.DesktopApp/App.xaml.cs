using ClubHouse.Bootstrapper.Unity.Extensions;
using ClubHouse.UI.DesktopApp.Extensions;
using ClubHouse.UI.DesktopApp.Handler;
using ClubHouse.UI.DesktopApp.Views;

using Prism.Ioc;

using System.Windows;
using System.Windows.Threading;

namespace ClubHouse.UI.DesktopApp {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        public App() {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            MessageBox.Show(e.Exception.ToString());
        }

        protected override Window CreateShell() {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) {
            containerRegistry.AddServices()
                             .AddUI();
        }

        protected override void OnInitialized() {
            ClubHouseBootStrapper bootStrapper = Container.Resolve<ClubHouseBootStrapper>();
            bootStrapper.Run();
            base.OnInitialized();
        }

        protected override async void OnExit(ExitEventArgs e) {
            var roomService = Container.Resolve<RoomManagerService>();
            await roomService.LeaveRoom();
            roomService.Dispose();
            base.OnExit(e);
        }
    }
}
