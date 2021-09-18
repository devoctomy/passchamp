using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Services;
using System;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Test
{
    public class TestGraphPinOutputFunction : IGraphPinOutputFunction
    {
        public IPin Execute(
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
            pin.Name = "Value";
            return pin;
        }

        public bool IsApplicable(string key)
        {
            return key.Equals(
                "TestGraphPinOutputFunction",
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
