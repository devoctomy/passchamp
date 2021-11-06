using System.Collections.Generic;
using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public interface IArgumentMapperService
    {
        void MapArguments<T>(
            T optionsInstance,
            Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions,
            string argumentString,
            List<CommandLineParserOptionAttribute> allSetOptions);
    }
}
