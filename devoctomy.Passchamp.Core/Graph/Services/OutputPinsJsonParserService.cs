using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services;

public class OutputPinsJsonParserService : IOutputPinsJsonParserService
{
    public Dictionary<string, IPin> Parse(JArray json)
    {
        var pins = new Dictionary<string, IPin>();
        foreach (var curPinJson in json)
        {
            var key = curPinJson["Key"].Value<string>();
            pins.Add(
                key,
                DataPinFactory.Instance.Create(
                    key,
                    new DataPinIntermediateValue(curPinJson["Value"].Value<string>()),
                    typeof(DataPinIntermediateValue)));
        }

        return pins;
    }
}
