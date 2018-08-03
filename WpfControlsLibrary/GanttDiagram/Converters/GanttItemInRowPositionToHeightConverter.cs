using System;
using System.Globalization;
using System.Windows.Data;
using WpfControlsLibrary.GanttDiagram.Models;

namespace WpfControlsLibrary.GanttDiagram.Converters
{
    internal class GanttItemInRowPositionToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GanttItemInRowPosition inRowPosition)
            {
                switch (inRowPosition)
                {
                    case GanttItemInRowPosition.FullRow:
                        return 80;
                    case GanttItemInRowPosition.BottomHalf:
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
    }
}
