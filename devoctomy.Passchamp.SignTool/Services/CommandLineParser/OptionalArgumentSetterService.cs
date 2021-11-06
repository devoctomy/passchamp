using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public class OptionalArgumentSetterService : IOptionalArgumentSetterService
    {
        private readonly IPropertyValueSetterService _propertyValueSetter;

        public OptionalArgumentSetterService(IPropertyValueSetterService propertyValueSetter)
        {
            _propertyValueSetter = propertyValueSetter;
        }

        public void SetOptionalValues<T>(
            T optionsInstance,
            Dictionary<PropertyInfo, CommandLineParserOptionAttribute> allOptions)
        {
            var optional = allOptions.Where(x => !x.Value.Required).ToList();
            foreach (var curOptional in optional)
            {
                _propertyValueSetter.SetPropertyValue(
                    optionsInstance,
                    curOptional.Key,
                    curOptional.Value.DefaultValue.ToString());
            }
        }
    }
}
