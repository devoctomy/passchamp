using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services.GraphPinOutputFunctions
{
    public class JsonSerialiserPinOutputFunction : IGraphPinOutputFunction
    {
        public bool IsApplicable(string key)
        {
            return key.Equals(
                "JsonSerialiserPinOutputFunction",
                StringComparison.InvariantCultureIgnoreCase);
        }

        public IPin Execute(
            string curNodeKey,
            string value,
            IReadOnlyDictionary<string, INode> nodes)
        {
            var pathParts = value.Split(".");
            var nodeName = pathParts[1];
            if (!nodes.ContainsKey(nodeName))
            {
                throw new KeyNotFoundException($"No node found in graph with the key '{nodeName}'.");
            }

            var node = nodes[nodeName];
            var outputPinName = pathParts[2];
            var pin = node.Output[outputPinName];
            return DataPinFactory.Instance.Create(
                "Value",
                JsonConvert.SerializeObject(
                    pin.ObjectValue,
                    Formatting.Indented));
        }
    }
}
