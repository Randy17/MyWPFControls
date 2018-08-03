using System;
using System.Globalization;
using System.Windows.Data;
using WpfControlsLibrary.GanttDiagram.Models;

namespace WpfControlsLibrary.GanttDiagram.Converters
{
    internal class GenttItemInRowPositionToBottomMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GanttItemInRowPosition inRowPosition)
            {
                switch (inRowPosition)
                {
                    case GanttItemInRowPosition.FullRow:
                    case GanttItemInRowPosition.BottomHalf:
                        return 0;
                    case GanttItemInRowPosition.UpperHalf:
                        return 40;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        //{
        //    if (values == null || values.Length != 2)
        //    {
        //        throw new ArgumentException("Argument 'values' is null or its length is not 2");
        //    }

        //    if (values[0] is GanttItemInRowPosition inRowPosition
        //        && values[1] is bool isShrinked)
        //    {
        //        switch (inRowPosition)
        //        {
        //            case GanttItemInRowPosition.FullRow:
        //            case GanttItemInRowPosition.BottomHalf:
        //                return 0;
        //            case GanttItemInRowPosition.UpperHalf:
        //                return isShrinked ? 20 : 40;
        //        }
        //    }

        //    throw new ArgumentException("Values dataTypes are unexpected");
        //}

        //public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
