using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ClubHouse.UI.DesktopApp.Converters {
    public class MaderatorActionVisibilityConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            var items = values.OfType<bool>();
            if (!items.Any())
                return Visibility.Visible;

            if (items.First() == false)
                return Visibility.Collapsed;

            foreach (var item in items.Skip(1))
                if (item)
                    return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
