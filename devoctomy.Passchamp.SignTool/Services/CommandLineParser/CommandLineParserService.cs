using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public class CommandLineParserService : ICommandLineParserService
    {
        private readonly ISingleArgumentParserService _singleArgumentParser;
        private readonly IDefaultArgumentParserService _defaultArgumentParserService;
        private readonly IArgumentMapperService _argumentMapper;
        private readonly IOptionalArgumentSetterService _optionalArgumentSetterSevice;

        public CommandLineParserService(
            ISingleArgumentParserService singleArgumentParser,
            IDefaultArgumentParserService defaultArgumentParserService,
            IArgumentMapperService arumentMapper,
            IOptionalArgumentSetterService optionalArgumentSetterSevice)
        {
            _singleArgumentParser = singleArgumentParser;
            _defaultArgumentParserService = defaultArgumentParserService;
            _argumentMapper = arumentMapper;
            _optionalArgumentSetterSevice = optionalArgumentSetterSevice;
        }

        public static CommandLineParserService CreateDefaultInstance()
        {
            var propertyValueSetterService = new PropertyValueSetterService();
            return new CommandLineParserService(
                new SingleArgumentParserService(),
                new DefaultArgumentParserService(propertyValueSetterService),
                new ArgumentMapperService(
                    new ArgumentMapperOptions(),
                    new SingleArgumentParserService(),
                    propertyValueSetterService),
                new OptionalArgumentSetterService(propertyValueSetterService));
        }

        public bool TryParseArgumentsAsOptions<T>(string argumentString, out ParseResults<T> results)
        {
            if (string.IsNullOrWhiteSpace(argumentString))
            {
                results = default(ParseResults<T>);
                return false;
            }

            results = new ParseResults<T>
            {
                Options = Activator.CreateInstance<T>()
            };
            var allOptions = GetAllOptions<T>();
            var allSetOptions = new List<CommandLineParserOptionAttribute>();

            _defaultArgumentParserService.SetDefaultOption<T>(
                results.Options,
                allOptions,
                ref argumentString,
                allSetOptions);

            _optionalArgumentSetterSevice.SetOptionalValues<T>(
                results.Options,
                allOptions);

            _argumentMapper.MapArguments(
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

        private Dictionary<PropertyInfo, CommandLineParserOptionAttribute> GetAllOptions<T>()
        {
            var propeties = new Dictionary<PropertyInfo, CommandLineParserOptionAttribute>();
            var allProperties = typeof(T).GetProperties();
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
}
