using devoctomy.Passchamp.Core.Graph.Data;
using devoctomy.Passchamp.Core.Graph.Services;
using System;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph
{
    public class DataPinFactory : IDataPinFactory
    {
        private readonly Dictionary<string, Type> _typeMap = new Dictionary<string, Type>
        {
            { "Boolean", typeof(bool) },
            { "String", typeof(string) },
            { "Int32", typeof(int) },
            { "Single", typeof(float) },
            { "Double", typeof(double) },
            { "Byte[]", typeof(byte[]) },
            { "DataPinIntermediateValue", typeof(DataPinIntermediateValue) },
            { "DataParserSection", typeof(DataParserSection) },
            { "Vault", typeof(Core.Vault.Vault) }
        };

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
            if(_typeMap.TryGetValue(valueType.Name, out var type))
            {
                var typeArgs = new []{ type };
                Type dataPinGenericType = typeof(DataPin<>);
                Type dataPinType = dataPinGenericType.MakeGenericType(typeArgs);

                return (IPin)Activator.CreateInstance(
                    dataPinType,
                    name,
                    value);
            }

            if(valueType.Name == "List`1")
            {
                var listTypeName = valueType.GenericTypeArguments[0].Name;
                if (_typeMap.TryGetValue(listTypeName, out var listType))
                {
                    var dataPinListTypeArgs = new[] { listType };
                    Type listGenericType = typeof(List<>);
                    Type datapinListType = listGenericType.MakeGenericType(dataPinListTypeArgs);
                    Type dataPinGenericType = typeof(DataPin<>);

                    var dataPinTypeArgs = new[] { datapinListType };
                    Type dataPinType = dataPinGenericType.MakeGenericType(dataPinTypeArgs);

                    return (IPin)Activator.CreateInstance(
                        dataPinType,
                        name,
                        value);
                }
            }

            throw new NotSupportedException($"Value type {valueType.Name} not supported.");
        }
    }
}
