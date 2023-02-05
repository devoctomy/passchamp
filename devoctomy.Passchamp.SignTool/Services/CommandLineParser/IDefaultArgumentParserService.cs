using System;
using System.Collections.Generic;
using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser;

public interface IDefaultArgumentParserService
{
    bool SetDefaultOption<T>(
        T optionsInstance,
        Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions,
        ref string argumentString,
        List<CommandLineParserOptionAttribute> allSetOptions,
        ref string invalidValue);

    bool SetDefaultOption(
        Type optionsType,
        object optionsInstance,
        Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions,
        ref string argumentString,
        List<CommandLineParserOptionAttribute> allSetOptions,
        ref string invalidValue);
}
