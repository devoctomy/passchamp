using devoctomy.Passchamp.Core.Graph.Data;
using System;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services.GraphPinPrepFunctions
{
    public class DataParserSectionGetterPinPrepFunction : IGraphPinPrepFunction
    {
        public bool IsApplicable(string key)
        {
            return key.Equals(
                "GetDataParserSectionValue",
                StringComparison.InvariantCultureIgnoreCase);
        }

        public IPin Execute(
            string curNodeKey,
            string value,
            IReadOnlyDictionary<string, IPin> inputPins,
            IReadOnlyDictionary<string, INode> nodes)
        {
            var pathParts = value.Split(".");
            var nodeName = pathParts[1];
            if(!nodes.ContainsKey(nodeName))
            {
                throw new KeyNotFoundException($"No node found in graph with the key '{nodeName}'.");
            }

            if (nodes[nodeName] is not DataParserNode node)
            {
                throw new InvalidOperationException($"Node '{nodeName}' is not of type DataParserNode.");
            }

            return node.GetSectionValue(pathParts[2]);
        }
    }
}
