using System.Globalization;

namespace devoctomy.Passchamp.Client.Converters
{
    public class EnumEqualityConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            var enumType = value.GetType();
            var compValue = Enum.Parse(enumType, parameter.ToString());
            return value == compValue;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
