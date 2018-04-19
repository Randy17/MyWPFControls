using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfControlsLibrary.GanttDiagram.Converters
{
    internal class IntToLeftMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return new Thickness((int) value, 5, 0, 0);

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return (int) ((Thickness)value).Left;

            return 0;
        }
    }
}
