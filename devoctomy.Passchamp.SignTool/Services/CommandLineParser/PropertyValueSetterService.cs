using System;
using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public class PropertyValueSetterService : IPropertyValueSetterService
    {
        public bool SetPropertyValue<T>(
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
                        return true;
                    }

                case "Boolean":
                    {
                        property.SetValue(
                            option,
                            bool.Parse(value));
                        return true;
                    }

                case "Int32":
                    {
                        property.SetValue(
                            option,
                            int.Parse(value));
                        return true;
                    }

                case "Single":
                    {
                        property.SetValue(
                            option,
                            float.Parse(value));
                        return true;
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
                                return true;
                            }

                            return false;
                        }

                        throw new NotSupportedException($"Destination type '{destType.Name}' is not supported.");
                    }
            };
        }
    }
}
