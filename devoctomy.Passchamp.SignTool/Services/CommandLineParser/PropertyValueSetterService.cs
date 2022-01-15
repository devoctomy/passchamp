using System;
using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public class PropertyValueSetterService : IPropertyValueSetterService
    {
        public void SetPropertyValue<T>(
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
                        if (destType.BaseType.Name.Equals("enum", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if(Enum.TryParse(destType, value, true, out var result))
                            {
                                property.SetValue(
                                    option,
                                    result);
                            }

                            return;
                        }

                        throw new NotSupportedException($"Destination type '{destType.Name}' is not supported.");
                    }
            };
        }
    }
}
