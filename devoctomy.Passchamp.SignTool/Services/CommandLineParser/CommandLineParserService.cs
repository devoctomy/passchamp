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
        private readonly IArgumentMapper _argumentMapper;
        private readonly IOptionalArgumentSetterService _optionalArgumentSetterSevice;

        public CommandLineParserService(
            ISingleArgumentParserService singleArgumentParser,
            IDefaultArgumentParserService defaultArgumentParserService,
            IArgumentMapper arumentMapper,
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
                new ArgumentMapper(
                    new ArgumentMapperOptions(),
                    new SingleArgumentParserService(),
                    propertyValueSetterService),
                new OptionalArgumentSetterService(propertyValueSetterService));
        }

        public T ParseArgumentsAsOptions<T>(string argumentString)
        {
            if(string.IsNullOrWhiteSpace(argumentString))
            {
                return default(T);
            }

            var optionsInstance = Activator.CreateInstance<T>();
            var allOptions = GetAllOptions<T>();
            var allSetOptions = new List<CommandLineParserOptionAttribute>();

            _defaultArgumentParserService.SetDefaultOption<T>(
                optionsInstance,
                allOptions,
                ref argumentString,
                allSetOptions);

            _optionalArgumentSetterSevice.SetOptionalValues<T>(
                optionsInstance,
                allOptions);

            _argumentMapper.MapArguments(
                optionsInstance,
                allOptions,
                argumentString,
                allSetOptions);

            var missingRequired = allOptions.Where(x => x.Value.Required && !allSetOptions.Contains(x.Value)).ToList();
            if(missingRequired.Any())
            {
                throw new ArgumentException($"Required arguments missing ({string.Join(',', missingRequired.Select(x => x.Value.LongName))}).");
            }

            return optionsInstance;
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
