using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Services;
using System;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Test;

public class TestGraphPinPrepFunction : IGraphPinPrepFunction
{
    public bool IsApplicable(string key)
    {
        return key.Equals(
            "TestGraphPinPrepFunction",
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
        if (!nodes.ContainsKey(nodeName))
        {
            throw new KeyNotFoundException($"No node found in graph with the key '{nodeName}'.");
        }

        var node = nodes[nodeName];
        var outputPin = node.Output[pathParts[2]];
        return outputPin;
    }
}
