using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services
{
    public class PinsJsonParserService : IPinsJsonParserService
    {
        public Dictionary<string, IDataPin> Parse(JArray json)
        {
            var pins = new Dictionary<string, IDataPin>();
            foreach (var curPinJson in json)
            {
                object value = null;
                var valueType = Type.GetType(curPinJson["Type"].Value<string>());
                if(valueType == null)
                {
                    throw new TypeLoadException($"Unknown type '{curPinJson["Type"].Value<string>()}'.");
                }
                switch (valueType.Name)
                {
                    case "String":
                        {
                            value = curPinJson["Value"].Value<string>();
                            break;
                        }
                    case "Int32":
                        {
                            value = curPinJson["Value"].Value<int>();
                            break;
                        }
                    default:
                        {
                            throw new NotSupportedException($"Pin value type '{curPinJson["Type"].Value<string>()}' not supported.");
                        }
                }
                var key = curPinJson["Key"].Value<string>();
                pins.Add(
                    key,
                    new DataPin(
                        key,
                        value,
                        valueType));
            }

            return pins;
        }
    }
}
