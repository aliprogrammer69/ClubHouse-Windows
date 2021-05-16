using System.Windows.Controls;

namespace ClubHouse.UI.DesktopApp.Views {
    /// <summary>
    /// Interaction logic for RoomListView.xaml
    /// </summary>
    public partial class RoomListView : UserControl {
        public RoomListView() {
            InitializeComponent();
        }

        private void ListBox_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            e.Handled = true;
        }
    }
}
