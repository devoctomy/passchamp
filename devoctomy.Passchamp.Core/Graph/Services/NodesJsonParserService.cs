﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace devoctomy.Passchamp.Core.Graph.Services;

public class NodesJsonParserService : INodesJsonParserService
{
    private readonly IServiceProvider _serviceProvider;

    public NodesJsonParserService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public NodesJsonParserServiceParseResults Parse(JArray json)
    {
        var nodes = new Dictionary<string, INode>();
        var firstNodeJson = json.First() as JObject;
        var startNodeKey = DoParse(
            nodes,
            firstNodeJson,
            _serviceProvider).Key;
        return new NodesJsonParserServiceParseResults
        {
            Nodes = nodes,
            StartNodeKey = startNodeKey
        };
    }

    private static KeyValuePair<string, INode> DoParse(
        Dictionary<string, INode> nodes,
        JObject curNode,
        IServiceProvider serviceProvider)
    {
        var curKey = curNode["Key"].Value<string>();
        var type = curNode["Type"].Value<string>().Split(":");
        if (type.Length != 2)
        {
            throw new TypeLoadException("Node type name should be 'Assembly:Type'.");
        }

        var nodeAssembly = Assembly.Load(type[0]);
        var nodeType = nodeAssembly.GetType(type[1]);
        var node = type.Length == 2 ? serviceProvider.GetService(nodeType) : null;
        if (node == null)
        {
            throw new TypeLoadException($"Could not activate instance of '{type[0]}:{type[1]}'.");
        }
        ((INode)node).NodeKey = curKey;
        var inode = node as INode;

        var inputsJson = curNode["Inputs"].Value<JArray>();
        foreach(var curInputJson in inputsJson)
        {
            var key = curInputJson["Key"].Value<string>();
            inode.PrepareInputDataPin(key, typeof(string), true);
            inode.Input[key] = DataPinFactory.Instance.Create(
                key,
                new DataPinIntermediateValue(curInputJson["Value"].Value<string>()),
                typeof(DataPinIntermediateValue));
        }

        var curNodeKey = curNode["Key"].Value<string>();
        nodes.Add(
            curNodeKey,
            inode);

        if (curNode.ContainsKey("Next"))
        {
            var nextNode = curNode["Next"].Value<JObject>();
            if(nextNode.Children().Any())
            {
                inode.NextKey = DoParse(
                    nodes,
                    nextNode,
                    serviceProvider).Key;
            }
        }

        return new KeyValuePair<string, INode>(
            curNodeKey,
            inode);
    }
}
