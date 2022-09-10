using System.Globalization;

namespace devoctomy.Passchamp.Maui.Converters;

public class BoolResourceColourConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        var valueBool = (bool)value;
        var colourNames = parameter.ToString().Split(',');
        var colourName = colourNames[valueBool ? 1 : 0];
        var found = Application.Current.Resources.TryGetValue(colourName, out var colour);
        if(!found)
        {
            var colourFields = typeof(Colors).GetFields();
            var colourField = colourFields.SingleOrDefault(x => x.Name == colourName);
            colour = colourField.GetValue(null);
        }

        return (Color)colour;
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
