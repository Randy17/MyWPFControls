using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfControlsLibrary.Infrastrucrure.Converters
{
    public class IntsToMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 4)
            {
                throw new ArgumentException("Argument 'values' is null or its length is not 4");
            }

            if (values[0] is int leftMargin
                && values[1] is int topMargin
                && values[2] is int rightMargin
                && values[3] is int bottomMargin)
            {
                return new Thickness(leftMargin, topMargin, rightMargin, bottomMargin);
            }
            else
            {
                throw new ArgumentException("Values are not ints");
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness)
                return new object[] {thickness.Left, thickness.Top, thickness.Right, thickness.Bottom};

            return null;
        }
    }
}
