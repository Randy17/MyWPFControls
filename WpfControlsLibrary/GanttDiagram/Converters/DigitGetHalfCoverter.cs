using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlsLibrary.GanttDiagram.Converters
{
    public class DigitGetHalfCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && double.TryParse(value.ToString(), out double dblValue))
            {
                return dblValue / 2;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
