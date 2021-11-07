using System;
using System.Collections.Generic;
using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public interface IDefaultArgumentParserService
    {
        void SetDefaultOption<T>(
            T optionsInstance,
            Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions,
            ref string argumentString,
            List<CommandLineParserOptionAttribute> allSetOptions);

        void SetDefaultOption(
            Type optionsType,
            object optionsInstance,
            Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions,
            ref string argumentString,
            List<CommandLineParserOptionAttribute> allSetOptions);
    }
}
