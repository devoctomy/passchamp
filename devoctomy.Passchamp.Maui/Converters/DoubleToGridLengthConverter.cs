using System.Globalization;

namespace devoctomy.Passchamp.Maui.Converters
{
    public class DoubleToGridLengthConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            var unit = Enum.Parse<GridUnitType>((string)parameter);
            return new GridLength((double)value, unit);
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
