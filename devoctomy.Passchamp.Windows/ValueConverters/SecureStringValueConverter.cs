using System;
using System.Globalization;
using System.Security;
using System.Windows.Data;

namespace devoctomy.Passchamp.Windows.ValueConverters
{
    public class SecureStringValueConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            var secureString = new SecureString();
            foreach(var curChar in ((string)value).ToCharArray())
            {
                secureString.AppendChar(curChar);
            }

            return secureString;
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
