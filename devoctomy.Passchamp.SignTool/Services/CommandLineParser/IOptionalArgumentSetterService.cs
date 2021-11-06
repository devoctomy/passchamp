using System.Collections.Generic;
using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public interface IOptionalArgumentSetterService
    {
        void SetOptionalValues<T>(
            T optionsInstance,
            Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions);
    }
}
