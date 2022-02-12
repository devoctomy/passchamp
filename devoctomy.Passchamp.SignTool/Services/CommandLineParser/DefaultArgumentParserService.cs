using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public class DefaultArgumentParserService : IDefaultArgumentParserService
    {
        private readonly IPropertyValueSetterService _propertyValueSetter;

        public DefaultArgumentParserService(IPropertyValueSetterService propertyValueSetter)
        {
            _propertyValueSetter = propertyValueSetter;
        }
        public bool SetDefaultOption<T>(
            T optionsInstance,
            Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions,
            ref string argumentString,
            List<CommandLineParserOptionAttribute> allSetOptions,
            ref string invalidValue)
        {
            return SetDefaultOption(
                typeof(T),
                optionsInstance,
                allOptions,
                ref argumentString,
                allSetOptions,
                ref invalidValue);
        }

        public bool SetDefaultOption(
            Type optionsType,
            object optionsInstance,
            Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions,
            ref string argumentString,
            List<CommandLineParserOptionAttribute> allSetOptions,
            ref string invalidValue)
        {
            var defaultOptionValue = string.Empty;
            invalidValue = string.Empty;
            if (!argumentString.StartsWith("-"))
            {
                var argContainsSpace = argumentString.IndexOf(" ") > 0;
                defaultOptionValue = argContainsSpace
                    ? argumentString[..argumentString.IndexOf(" ")]
                    : argumentString;
                argumentString = argContainsSpace
                    ? argumentString[(argumentString.IndexOf(" ") + 1)..]
                    : String.Empty;
            }
            var defaultOption = allOptions.SingleOrDefault(x => x.Value.IsDefault);
            if (defaultOption.Key != null && !string.IsNullOrEmpty(defaultOptionValue))
            {
                if(_propertyValueSetter.SetPropertyValue(
                    optionsInstance,
                    defaultOption.Key,
                    defaultOptionValue))
                {
                    allSetOptions.Add(defaultOption.Value);
                    return true;
                }

                invalidValue = defaultOptionValue;
                return false;
            }

            return true;    // Default wasn't required
        }
    }
}
