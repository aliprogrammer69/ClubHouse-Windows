using Prism.Regions;

using System.Linq;

namespace ClubHouse.UI.DesktopApp.Extensions {
    public static class RegionNavigationServiceExtensions {
        public static void Close(this IRegionNavigationService navigationService) {
            object activeView = navigationService.Region.ActiveViews.FirstOrDefault();
            if (activeView != null) {
                navigationService.Region.Remove(activeView);
                foreach (var item in navigationService.Region.Views) {
                    if (item.GetType() == activeView.GetType())
                        navigationService.Region.Remove(item);
                }
            }

            if (navigationService.Region.Views.Any() && navigationService.Journal.CanGoBack)
                navigationService.Journal.GoBack();
            else
                navigationService.Journal.Clear();
        }
    }
}
