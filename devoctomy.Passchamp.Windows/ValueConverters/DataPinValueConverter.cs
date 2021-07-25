using devoctomy.Passchamp.Core.Graph;
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
            if(targetType != typeof(string))
            {
                throw new NotSupportedException($"Target type {targetType.Name} not supported by value converter.");
            }

            return value.ToString();
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(IPin))
            {
                throw new NotSupportedException($"Value must be of type IDataPin.");
            }

            return null;
        }
    }
}
