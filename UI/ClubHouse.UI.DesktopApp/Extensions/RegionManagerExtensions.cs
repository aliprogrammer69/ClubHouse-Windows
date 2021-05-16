using ClubHouse.Common;
using ClubHouse.Domain.Models.Response;
using ClubHouse.UI.DesktopApp.Views;

using Prism.Regions;

namespace ClubHouse.UI.DesktopApp.Extensions {
    public static class RegionManagerExtensions {
        public static void ShowProfile(this IRegionManager regionManager, string region, long userId) {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(Consts.ProfileUserIdNavigationParameterKey, userId);
            regionManager.RequestNavigate(region, nameof(ProfileView), parameters);
        }

        public static void ShowRoom(this IRegionManager regionManager, InitChannelResponse joinResult) {
            regionManager.RequestNavigate(Regions.RoomContainerRegionName, nameof(RoomView), new NavigationParameters() {
                    { Consts.ChannelResponseNavigationParameterKey, joinResult }
                });
        }

        public static void ShowErrorView(this IRegionManager regionManager, string message) {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(Consts.ErrorMessageNavigationParamtereKey, message);
            regionManager.RequestNavigate(Regions.MainRegionName, nameof(ErrorView), parameters);
        }

        public static void ShowMainView(this IRegionManager regionManager, GetProfileResponse profileResponse) {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(Consts.UserInfoNavigationParameterKey, profileResponse);
            regionManager.RequestNavigate(Regions.MainRegionName, nameof(MainView), parameters);
        }

        public static void ShowLoginView(this IRegionManager regionManager) {
            regionManager.RequestNavigate(Regions.MainRegionName, nameof(LoginView));
        }
    }
}
