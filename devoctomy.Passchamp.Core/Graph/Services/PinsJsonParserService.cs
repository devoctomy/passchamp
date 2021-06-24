﻿using Newtonsoft.Json.Linq;
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
                switch (curPinJson["Type"].Value<string>())
                {
                    case "String":
                        {
                            value = curPinJson["Value"].Value<string>();
                            break;
                        }
                    case "Int":
                        {
                            value = curPinJson["Value"].Value<int>();
                            break;
                        }
                    default:
                        {
                            throw new NotSupportedException($"Pin value type '{curPinJson["Type"].Value<string>()}' not supported.");
                        }
                }
                pins.Add(
                    curPinJson["Key"].Value<string>(),
                    new DataPin(value));
            }

            return pins;
        }
    }
}
