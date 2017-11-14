using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfControlsLibrary.Infrastrucrure.Converters
{
    public class CollectionToMaxColumnsCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ICollection collection = value as ICollection;
            if (collection == null)
                return 1;

            int maxCount = 0;

            if (!int.TryParse(parameter.ToString(), out maxCount))
                return collection.Count;

            return maxCount > collection.Count ? collection.Count : maxCount;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
