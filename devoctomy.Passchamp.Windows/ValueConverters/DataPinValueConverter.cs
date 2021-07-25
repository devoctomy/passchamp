using System;
using System.Globalization;
using System.Windows.Data;

namespace devoctomy.Passchamp.Windows.ValueConverters
{
    public class DataPinValueConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {      
            return System.Convert.ChangeType(value, targetType);
        }
    }
}
