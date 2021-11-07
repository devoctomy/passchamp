using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public class ArgumentMapperService : IArgumentMapperService
    {
        private readonly ArgumentMapperOptions _options;
        private readonly ISingleArgumentParserService _singleArgumentParserService;
        private readonly IPropertyValueSetterService _propertyValueSetter;

        public ArgumentMapperService(
            ArgumentMapperOptions options,
            ISingleArgumentParserService singleArgumentParserService,
            IPropertyValueSetterService propertyValueSetter)
        {
            _options = options;
            _singleArgumentParserService = singleArgumentParserService;
            _propertyValueSetter = propertyValueSetter;
        }
        public void MapArguments<T>(
            T optionsInstance,
            Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions,
            string argumentString,
            List<CommandLineParserOptionAttribute> allSetOptions)
        {
            MapArguments(
                typeof(T),
                optionsInstance,
                allOptions,
                argumentString,
                allSetOptions);
        }

        public void MapArguments(
            Type optionsType,
            object optionsInstance,
            Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions,
            string argumentString,
            List<CommandLineParserOptionAttribute> allSetOptions)
        {
            var regex = new Regex(_options.Regex);
            var matches = regex.Matches(argumentString);
            var allMatches = matches.Select(x => x.Value.TrimEnd()).ToList();
            foreach (var curMatch in allMatches)
            {
                var match = curMatch.TrimStart().TrimStart('-');
                var argument = _singleArgumentParserService.Parse(match);
                var option = allOptions.SingleOrDefault(x => x.Value.ShortName == argument.Name || x.Value.LongName == argument.Name);
                if (option.Key != null)
                {
                    _propertyValueSetter.SetPropertyValue(
                        optionsInstance,
                        option.Key,
                        argument.Value);
                    allSetOptions.Add(option.Value);
                }
            }
        }
    }
}
