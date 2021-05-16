using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClubHouse.UI.DesktopApp.Views {
    /// <summary>
    /// Interaction logic for EditDialogView.xaml
    /// </summary>
    public partial class EditDialogView : UserControl {
        public EditDialogView() {
            InitializeComponent();
        }

        public static readonly DependencyProperty AcceptButtonCommandProperty = DependencyProperty.Register("AcceptButtonCommand",
                                                    typeof(ICommand), typeof(EditDialogView), new PropertyMetadata(AcceptButtonChanged));
        public ICommand AcceptButtonCommand {
            get { return (ICommand)GetValue(AcceptButtonCommandProperty); }
            set { SetValue(AcceptButtonCommandProperty, value); }
        }

        private static void AcceptButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((EditDialogView)d).AcceptButtonCommand = (ICommand)e.NewValue;
        }

        public static readonly DependencyProperty MaxLineProperty = DependencyProperty.Register("MaxLine",
                                                    typeof(int), typeof(EditDialogView), new PropertyMetadata(1, MaxLineChanged));
        public int MaxLine {
            get { return (int)GetValue(MaxLineProperty); }
            set { SetValue(MaxLineProperty, value); }
        }

        private static void MaxLineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((EditDialogView)d).MaxLine = (int)e.NewValue;
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint",
                                                    typeof(string), typeof(EditDialogView), new FrameworkPropertyMetadata());
        public string Hint {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }
    }
}
