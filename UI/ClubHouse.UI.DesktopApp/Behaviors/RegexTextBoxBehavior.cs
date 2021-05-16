using Microsoft.Xaml.Behaviors;

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ClubHouse.UI.DesktopApp.Behaviors {
    public class RegexTextBoxBehavior : Behavior<TextBox> {

        public static readonly DependencyProperty ExpersionProperty =
            DependencyProperty.Register("Expersion", typeof(string), typeof(RegexTextBoxBehavior),
            new FrameworkPropertyMetadata("*"));
        public string Expersion { get => (string)GetValue(ExpersionProperty); set => SetValue(ExpersionProperty, value); }

        protected override void OnAttached() {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            DataObject.AddPastingHandler(AssociatedObject, OnPaste);
        }

        protected override void OnDetaching() {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
            DataObject.RemovePastingHandler(AssociatedObject, OnPaste);
        }

        private void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            string text = string.Concat(AssociatedObject.Text, e.Text);
            e.Handled = !Regex.IsMatch(text, Expersion);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e) {
            if (e.DataObject.GetDataPresent(DataFormats.Text)) {
                string text = Convert.ToString(e.DataObject.GetData(DataFormats.Text));
                if (!Regex.IsMatch(text, Expersion))
                    e.CancelCommand();
            }
            else
                e.CancelCommand();

        }
    }
}
