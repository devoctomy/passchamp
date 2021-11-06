using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public interface IPropertyValueSetterService
    {
        void SetPropertyValue<T>(
            T option,
            PropertyInfo property,
            string value);
    }
}
