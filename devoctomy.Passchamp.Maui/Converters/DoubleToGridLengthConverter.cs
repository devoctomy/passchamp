using System.Globalization;

namespace devoctomy.Passchamp.Maui.Converters;

public class DoubleToGridLengthConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        var unit = Enum.Parse<GridUnitType>(parameter != null ?(string)parameter : "Absolute");
        if(unit == GridUnitType.Absolute)
        {
            var length = System.Convert.ToDouble(value);
            return new GridLength(length, unit);
        }

        return new GridLength(0, unit);
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
