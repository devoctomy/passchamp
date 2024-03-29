﻿using devoctomy.Passchamp.Core.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;

namespace devoctomy.Passchamp.Core.Graph.Services;

public class InputPinsJsonParserService : IInputPinsJsonParserService
{
    private readonly ITypeResolverService _typeResolverService;

    public InputPinsJsonParserService(ITypeResolverService typeResolverService)
    {
        _typeResolverService = typeResolverService;
    }

    public Dictionary<string, IPin> Parse(JArray json)
    {
        var pins = new Dictionary<string, IPin>();
        foreach (var curPinJson in json)
        {
            object value = null;
            var valueType = _typeResolverService.GetType(curPinJson["Type"].Value<string>());
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

                case "SecureString":
                    {
                        value = new NetworkCredential(null, curPinJson["Value"].Value<string>()).SecurePassword;
                        break;
                    }

                default:
                    {
                        throw new NotSupportedException($"Pin value type '{valueType.Name}' not supported.");
                    }
            }
            var key = curPinJson["Key"].Value<string>();
            pins.Add(
                key,
                DataPinFactory.Instance.Create(
                    key,
                    value,
                    valueType));
        }

        return pins;
    }
}
