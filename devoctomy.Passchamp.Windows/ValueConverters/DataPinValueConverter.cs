using devoctomy.Passchamp.Core.Graph;
using System;
using System.Globalization;
using System.Windows.Data;

namespace devoctomy.Passchamp.Windows.ValueConverters
{
    public class DataPinValueConverter : IMultiValueConverter
    {
        public object Convert(
            object[] values,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (targetType != typeof(string))
            {
                throw new NotSupportedException($"Target type {targetType.Name} not supported by value converter.");
            }

            return ((IPin)values[0]).ObjectValue.ToString();
        }

        public object[] ConvertBack(
            object value,
            Type[] targetTypes,
            object parameter,
            CultureInfo culture)
        {
            var converted = System.Convert.ChangeType(value.ToString(), targetTypes[1]);
            return new object[] { DataPinFactory.Instance.Create(
                "test",
                converted,
                targetTypes[1]) };
        }
    }
}
