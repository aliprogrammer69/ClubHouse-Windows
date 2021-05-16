using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace ClubHouse.UI.DesktopApp.Converters {
    public class AnyValueVisibilityCollapsedConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            var items = values.OfType<bool>();
            if (!items.Any())
                return Visibility.Visible;

            foreach (var item in items)
                if (!item)
                    return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
