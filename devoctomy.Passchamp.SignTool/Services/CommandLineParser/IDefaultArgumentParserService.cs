using System;
using System.Collections.Generic;
using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser;

public interface IDefaultArgumentParserService
{
    DefaultArgumentParserServiceSetDefaultOptionResult SetDefaultOption<T>(
        T optionsInstance,
        Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions,
        string argumentString,
        List<CommandLineParserOptionAttribute> allSetOptions);

    DefaultArgumentParserServiceSetDefaultOptionResult SetDefaultOption(
        Type optionsType,
        object optionsInstance,
        Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions,
        string argumentString,
        List<CommandLineParserOptionAttribute> allSetOptions);
}
