﻿using System;
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

        public void SetDefaultOption<T>(
            T optionsInstance,
            Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions,
            ref string argumentString,
            List<CommandLineParserOptionAttribute> allSetOptions)
        {
            var defaultOptionValue = string.Empty;
            if (!argumentString.StartsWith("-"))
            {
                defaultOptionValue = argumentString.Substring(
                    0,
                    argumentString.IndexOf(" "));
                argumentString = argumentString.Substring(argumentString.IndexOf(" ") + 1);
            }
            var defaultOption = allOptions.SingleOrDefault(x => x.Value.IsDefault);
            if (defaultOption.Key != null && !string.IsNullOrEmpty(defaultOptionValue))
            {
                _propertyValueSetter.SetPropertyValue(
                    optionsInstance,
                    defaultOption.Key,
                    defaultOptionValue);
                allSetOptions.Add(defaultOption.Value);
            }
        }
    }
}