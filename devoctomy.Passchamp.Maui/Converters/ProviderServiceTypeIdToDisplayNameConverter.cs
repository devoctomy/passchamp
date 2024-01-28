using devoctomy.Passchamp.Core.Cloud.Utility;
using System.Globalization;

namespace devoctomy.Passchamp.Maui.Converters;

public class ProviderServiceTypeIdToDisplayNameConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        var attribute = CloudStorageProviderServiceAttributeUtility.Get((string)value);
        return attribute.DisplayName;
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
