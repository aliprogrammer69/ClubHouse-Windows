using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClubHouse.UI.DesktopApp.Views {
    /// <summary>
    /// Interaction logic for InviteToChannelView.xaml
    /// </summary>
    public partial class InviteToChannelView : UserControl {
        public InviteToChannelView() {
            InitializeComponent();
            this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight * 0.5);
            this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.25);
        }
    }
}
