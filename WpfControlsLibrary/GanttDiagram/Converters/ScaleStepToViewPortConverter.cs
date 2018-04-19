using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfControlsLibrary.GanttDiagram.Converters
{
    internal class ScaleStepToViewPortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return new Rect(0, 0, (int) value, (int) value);

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
