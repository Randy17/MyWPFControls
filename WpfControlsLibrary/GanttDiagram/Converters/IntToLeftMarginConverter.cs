using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WpfControlsLibrary.GanttDiagram.Models;

namespace WpfControlsLibrary.GanttDiagram.Converters
{
    internal class IntToLeftMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int bottomMargin = 0;
            if (parameter is GanttItemInRowPosition inRowPosition)
            {
                if (inRowPosition == GanttItemInRowPosition.UpperHalf)
                    bottomMargin = 40;
            }

            if (value != null)
                return new Thickness((int) value, 5, 0, bottomMargin);

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
