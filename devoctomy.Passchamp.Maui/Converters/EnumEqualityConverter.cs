using System.Globalization;

namespace devoctomy.Passchamp.Maui.Converters;

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
        return value.Equals(compValue);
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
