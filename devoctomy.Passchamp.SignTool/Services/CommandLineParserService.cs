using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace devoctomy.Passchamp.SignTool.Services
{
    public class CommandLineParserService : ICommandLineParserService
    {
        public static string Regex => Properties.Resources.CommandLineParserRegex;

        private readonly ISingleArgumentParser _singleArgumentParser;

        public CommandLineParserService(ISingleArgumentParser singleArgumentParser)
        {
            _singleArgumentParser = singleArgumentParser;
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

            var optional = allOptions.Where(x => !x.Value.Required).ToList();
            foreach(var curOptional in optional)
            {
                SetOption(optionsInstance, curOptional.Key, curOptional.Value.DefaultValue.ToString());
            }

            var defaultOptionValue = string.Empty;
            if(!argumentString.StartsWith("-"))
            {
                defaultOptionValue = argumentString.Substring(
                    0,
                    argumentString.IndexOf(" "));
                argumentString = argumentString.Substring(argumentString.IndexOf(" ") + 1);
            }
            var defaultOption = allOptions.SingleOrDefault(x => x.Value.IsDefault);
            if(defaultOption.Key != null && !string.IsNullOrEmpty(defaultOptionValue))
            {
                SetOption(
                    optionsInstance,
                    defaultOption.Key,
                    defaultOptionValue);
                allSetOptions.Add(defaultOption.Value);
            }

            var regex = new Regex(Regex);
            var matches = regex.Matches(argumentString);
            var allMatches = matches.Select(x => x.Value.TrimEnd()).ToList();            
            foreach(var curMatch in allMatches)
            {
                var match = curMatch.TrimStart().TrimStart('-');
                var argument = _singleArgumentParser.Parse(match);
                var option = allOptions.SingleOrDefault(x => x.Value.ShortName == argument.Name || x.Value.LongName == argument.Name);
                if(option.Key != null)
                {
                    SetOption(
                        optionsInstance,
                        option.Key,
                        argument.Value);
                    allSetOptions.Add(option.Value);
                }

            }

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

        private void SetOption<T>(
            T option,
            PropertyInfo property,
            string value)
        {
            var destType = property.PropertyType;
            switch (destType.Name)
            {
                case "String":
                    {
                        property.SetValue(
                            option,
                            value);
                        break;
                    }

                case "Boolean":
                    {
                        property.SetValue(
                            option,
                            bool.Parse(value));
                        break;
                    }

                case "Int32":
                    {
                        property.SetValue(
                            option,
                            int.Parse(value));
                        break;
                    }

                case "Single":
                    {
                        property.SetValue(
                            option,
                            float.Parse(value));
                        break;
                    }

                default:
                    {
                        throw new NotSupportedException($"Destination type '{destType.Name}' is not supported.");
                    }
            };
        }
    }
}
