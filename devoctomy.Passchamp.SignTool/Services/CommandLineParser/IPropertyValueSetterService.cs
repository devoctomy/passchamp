using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public interface IPropertyValueSetterService
    {
        bool SetPropertyValue<T>(
            T option,
            PropertyInfo property,
            string value);
    }
}
