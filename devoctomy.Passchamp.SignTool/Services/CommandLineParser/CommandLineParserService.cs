using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser;

public class CommandLineParserService : ICommandLineParserService
{
    private readonly IDefaultArgumentParserService _defaultArgumentParserService;
    private readonly IArgumentMapperService _argumentMapper;
    private readonly IOptionalArgumentSetterService _optionalArgumentSetterSevice;

    public CommandLineParserService(
        IDefaultArgumentParserService defaultArgumentParserService,
        IArgumentMapperService arumentMapper,
        IOptionalArgumentSetterService optionalArgumentSetterSevice)
    {
        _defaultArgumentParserService = defaultArgumentParserService;
        _argumentMapper = arumentMapper;
        _optionalArgumentSetterSevice = optionalArgumentSetterSevice;
    }

    public static CommandLineParserService CreateDefaultInstance()
    {
        var propertyValueSetterService = new PropertyValueSetterService();
        return new CommandLineParserService(
            new DefaultArgumentParserService(propertyValueSetterService),
            new ArgumentMapperService(
                new ArgumentMapperOptions(),
                new SingleArgumentParserService(),
                propertyValueSetterService),
            new OptionalArgumentSetterService(propertyValueSetterService));
    }

    public bool TryParseArgumentsAsOptions<T>(string argumentString, out ParseResults results)
    {
        return TryParseArgumentsAsOptions(
            typeof(T),
            argumentString,
            out results);
    }

    public bool TryParseArgumentsAsOptions(
        Type optionsType,
        string argumentString,
        out ParseResults results)
    {
        // This block is unlikely to happen at runtime as arguments string
        // will always contain application path
        if (string.IsNullOrWhiteSpace(argumentString))
        {
            results = default;
            return false;
        }

        results = new ParseResults
        {
            Options = Activator.CreateInstance(optionsType)
        };
        var allOptions = GetAllOptions(optionsType);
        var allSetOptions = new List<CommandLineParserOptionAttribute>();
        string invalidOption = string.Empty;
        if(!_defaultArgumentParserService.SetDefaultOption(
            optionsType,
            results.Options,
            allOptions,
            ref argumentString,
            allSetOptions,
            ref invalidOption))
        {
            var defaultOption = allOptions.SingleOrDefault(x => x.Value.IsDefault);
            results.Exception = new System.ArgumentException(
                $"Failed to set default argument '{defaultOption.Value.DisplayName}'.",
                $"{defaultOption.Value.DisplayName}");
            results.InvalidOptions.Add(defaultOption.Value.DisplayName, invalidOption);
            return false;
        }

        _optionalArgumentSetterSevice.SetOptionalValues(
            optionsType,
            results.Options,
            allOptions);

        _argumentMapper.MapArguments(
            optionsType,
            results.Options,
            allOptions,
            argumentString,
            allSetOptions);

        var missingRequired = allOptions.Where(x => x.Value.Required && !allSetOptions.Any(y => y.LongName == x.Value.LongName)).ToList();
        if (missingRequired.Any())
        {
            results.Exception = new ArgumentException($"Required arguments missing ({string.Join(',', missingRequired.Select(x => x.Value.LongName))}).");
        }

        return !missingRequired.Any();
    }

    private static Dictionary<PropertyInfo, CommandLineParserOptionAttribute> GetAllOptions(Type optionsType)
    {
        var propeties = new Dictionary<PropertyInfo, CommandLineParserOptionAttribute>();
        var allProperties = optionsType.GetProperties();
        foreach (var curProperty in allProperties)
        {
            var optionAttribute = (CommandLineParserOptionAttribute)curProperty.GetCustomAttributes(typeof(CommandLineParserOptionAttribute), true).FirstOrDefault();
            if (optionAttribute != null)
            {
                propeties.Add(
                    curProperty,
                    optionAttribute);
            }
        }
        return propeties;
    }
}
