using System;
using System.Windows;
using System.Windows.Data;

namespace WpfControlsLibrary.Infrastrucrure.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? b = (bool?)value;
            if (b == true)
                return Visibility.Visible;

            if (b == false)
                return Visibility.Collapsed;

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
