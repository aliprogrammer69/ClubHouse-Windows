using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClubHouse.UI.DesktopApp.Handler {
    public static class ThreadManagerUtil {
        public static void RunInUI(Action action) {
            if(Application.Current?.Dispatcher != null) {
                Application.Current.Dispatcher.Invoke(action);
            }
        }
    }
}
