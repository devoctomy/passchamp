using devoctomy.Passchamp.Core.Enums;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Data;
using devoctomy.Passchamp.Core.Graph.Presets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace devoctomy.Passchamp.Core.Services;

public class GraphFactory(IEnumerable<IGraphPreset> graphPresets) : IGraphFactory
{
    private readonly IEnumerable<IGraphPreset> _graphPresets = graphPresets;

    public (IGraph encrypt, IGraph decrypt) LoadPresetSet(
        IGraphPresetSet presetSet,
        Func<Type, INode> instantiateNode,
        Dictionary<string, object> parameters)
    {
        var encrypt = LoadPreset(
            presetSet.EncryptPreset,
            instantiateNode,
            parameters);
        var decrypt = LoadPreset(
            presetSet.DecryptPreset,
            instantiateNode,
            parameters);
        return (encrypt, decrypt);
    }

    public IGraph LoadPreset(
        IGraphPreset preset,
        Func<Type, INode> instantiateNode,
        Dictionary<string, object> parameters)
    {
        var graphSettings = new GraphSettings
        {
            Author = preset.Author,
            Description = preset.Description,
        };

        var nodes = preset.OrderedNodes != null ?
            GetOrderedNodes(preset.OrderedNodes, instantiateNode) :
            GetUnorderedNodes(preset.UnorderedNodes, instantiateNode);

        AddConnections(
            nodes,
            preset.Connections);

        var inputPins = preset.InputPins;
        foreach (var curParameter in parameters)
        {
            if(inputPins.ContainsKey(curParameter.Key))
            {
                inputPins[curParameter.Key] = DataPinFactory.Instance.Create(curParameter.Key, curParameter.Value);
            }
        }

        return new Graph.Graph(
            graphSettings,
            inputPins,
            preset.OutputPins,
            nodes,
            preset.OrderedNodes != null ? nodes.First().Key : preset.StartKey,
            null,
            null);
    }

    public IGraph LoadNative(
        GraphContext context,
        NativeGraphs graph,
        Func<Type, INode> instantiateNode,
        Dictionary<string, object> parameters)
    {
        if (context == GraphContext.None)
        {
            throw new ArgumentException("Context of 'None' is not allowed.", nameof(context));
        }

        if (graph == NativeGraphs.None)
        {
            throw new ArgumentException("Graph of 'None' is not allowed.", nameof(graph));
        }

        switch (graph)
        {
            case NativeGraphs.Default:
                {
                    return context == GraphContext.Encrypt ?
                        LoadPreset(_graphPresets.Single(x => x.Context == GraphContext.Encrypt && x.Default), instantiateNode, parameters) :
                        LoadPreset(_graphPresets.Single(x => x.Context == GraphContext.Decrypt && x.Default), instantiateNode, parameters);
                }

            default:
                {
                    throw new NotSupportedException($"Native graph '{graph}' is not supported.");
                }
        }
    }

    private static void ConnectPins(
        Dictionary<string, INode> nodes,
        string nodeAName,
        string pinAName,
        string nodeBName,
        string pinBName)
    {
        var nodeA = nodes[nodeAName];
        var nodeB = nodes[nodeBName];

        if(pinBName.Contains(':'))
        {
            var pinNameParts = pinBName.Split(':');
            switch(pinNameParts[0])
            {
                case "DataParserSection":
                    {
                        var dataParserNode = nodeB as DataParserNode;
                        nodeA.Input[pinAName] = dataParserNode.GetSectionValue(pinNameParts[1]);
                        return;
                    }
            }
        }

        var nodeBPinPropInfo = nodeB.OutputPinsProperties[pinBName];
        var nodeBPinOutAttribute = (NodeOutputPinAttribute)Attribute.GetCustomAttribute(nodeBPinPropInfo, typeof(NodeOutputPinAttribute));
        nodeA.Input[pinAName] = nodeB.GetOutput(pinBName, nodeBPinOutAttribute.ValueType);
    }

    private static void AddConnections(
        Dictionary<string, INode> nodes,
        List<NodeConnection> connections)
    {
        foreach (var connection in connections) 
        {
            ConnectPins(
                nodes,
                connection.NodeToKey,
                connection.PinToKey,
                connection.NodeFromKey,
                connection.PinFromKey);
        }
    }

    private static Dictionary<string, INode> GetOrderedNodes(
        List<NodeRef> nodeRefs,
        Func<Type, INode> instantiateNode)
    {
        var nodes = new Dictionary<string, INode>();
        for (var i = 0;  i < nodeRefs.Count; i++)
        {
            var last = i == nodeRefs.Count - 1;
            var curNodeRef = nodeRefs[i];
            var node = instantiateNode(curNodeRef.NodeType);
            if(!last)
            {
                node.NextKey = nodeRefs[i + 1].Key;
            }

            if(curNodeRef.InputPins != null)
            {
                foreach (var curInputPin in curNodeRef.InputPins)
                {
                    node.Input[curInputPin.Key] = curInputPin.Value;
                }
            }

            nodes.Add(curNodeRef.Key, node);
        }

        return nodes;
    }

    private static Dictionary<string, INode> GetUnorderedNodes(
        List<NodeRef> nodeRefs,
        Func<Type, INode> instantiateNode)
    {
        var nodes = new Dictionary<string, INode>();
        for (var i = 0; i < nodeRefs.Count; i++)
        {
            var curNodeRef = nodeRefs[i];
            var node = instantiateNode(curNodeRef.NodeType);
            node.NextKey = curNodeRef.NextKey;

            if (curNodeRef.InputPins != null)
            {
                foreach (var curInputPin in curNodeRef.InputPins)
                {
                    node.Input[curInputPin.Key] = curInputPin.Value;
                }
            }

            nodes.Add(curNodeRef.Key, node);
        }

        return nodes;
    }
}
