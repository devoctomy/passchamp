using devoctomy.Passchamp.Core.Graph.Data;
using devoctomy.Passchamp.Core.Graph.Services;
using System;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph
{
    public class DataPinFactory : IDataPinFactory
    {
        private static IDataPinFactory _instance;
        public static IDataPinFactory Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new DataPinFactory();
                }

                return _instance;
            }
        }

        public IPin Create(
            string name,
            object value)
        {
            if(value == null)
            {
                throw new ArgumentException("Value cannot be null, use override with valueType instead.");
            }

            return Create(
                name,
                value,
                value.GetType());
        }

        public IPin Create(
            string name,
            object value,
            Type valueType)
        {
            switch (valueType.Name)
            {
                case "Boolean":
                    {
                        return new DataPin<bool>(
                            name,
                            (bool)value);
                    }
                case "String":
                    {
                        return new DataPin<string>(
                            name,
                            (string)value);
                    }
                case "Int32":
                    {
                        return new DataPin<int>(
                            name,
                            (int)value);
                    }
                case "Byte[]":
                    {
                        return new DataPin<byte[]>(
                            name,
                            (byte[])value);
                    }
                case "DataPinIntermediateValue":
                    {
                        return new DataPin<DataPinIntermediateValue>(
                            name,
                            (DataPinIntermediateValue)value);
                    }
                case "List`1":
                    {
                        var typeName = valueType.GenericTypeArguments[0].Name;
                        switch(typeName)
                        {
                            case "DataParserSection":
                                {
                                    return new DataPin<List<DataParserSection>>(
                                        name,
                                        (List<DataParserSection>)value);
                                }
                            default:
                                {
                                    throw new NotSupportedException($"Generic list of type {typeName} not supported.");
                                }
                        }
                    }
                default:
                    {
                        throw new NotSupportedException($"Value type {valueType.Name} not supported.");
                    }
            }
        }
    }
}
