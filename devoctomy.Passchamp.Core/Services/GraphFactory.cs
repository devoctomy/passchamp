using devoctomy.Passchamp.Core.Cryptography;
using devoctomy.Passchamp.Core.Enums;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using devoctomy.Passchamp.Core.Graph.Data;
using devoctomy.Passchamp.Core.Graph.IO;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Core.Graph.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace devoctomy.Passchamp.Core.Services;

public class GraphFactory : IGraphFactory
{
    private readonly ISecureStringUnpacker _secureStringUnpacker;

    public GraphFactory(ISecureStringUnpacker secureStringUnpacker)
    {
        _secureStringUnpacker = secureStringUnpacker;
    }

    public IGraph LoadNative(
        GraphContext context,
        NativeGraphs graph,
        params KeyValuePair<string, object>[] parameters)
    {
        switch(graph)
        {
            case NativeGraphs.Default:
                {
                    return context == GraphContext.Encrypt ? LoadDefaultEncrypt(parameters) : null;
                }

            default:
                {
                    throw new NotSupportedException($"Native graph '{graph}' is not supported.");
                }
        }
    }

    private IGraph LoadDefaultEncrypt(params KeyValuePair<string, object>[] parameters)
    {
        var inputPins = new Dictionary<string, IPin>
        {
            { "SaltLength", DataPinFactory.Instance.Create("SaltLength", 16) },
            { "IvLength", DataPinFactory.Instance.Create("IvLength", 16) },
            { "KeyLength", DataPinFactory.Instance.Create("KeyLength", 32) },
            { "Passphrase", DataPinFactory.Instance.Create("Passphrase", new NetworkCredential(null, GetParameterValue<string>("passphrase", parameters)).SecurePassword) },
            { "PlainText", DataPinFactory.Instance.Create("PlainText", GetParameterValue<string>("plaintext", parameters)) },
            { "OutputFileName", DataPinFactory.Instance.Create("OutputFileName", GetParameterValue<string>("outputfilename", parameters)) }
         };

        var nodes = new Dictionary<string, INode>
        {
            {
                "saltGenerator",
                new RandomByteArrayGeneratorNode
                {
                    NextKey = "ivGenerator",
                }
            },
            {
                "ivGenerator",
                new RandomByteArrayGeneratorNode
                {
                    NextKey = "deriveKey"
                }
            },
            {
                "deriveKey",
                new DeriveKeyFromPasswordExNode(_secureStringUnpacker)
                {
                    NextKey = "encode"
                }
            },
            {
                "encode",
                new Utf8EncoderNode
                {
                    NextKey = "encrypt"
                }
            },
            {
                "encrypt",
                new EncryptNode
                {
                    NextKey = "arrayJoiner"
                }
            },
            {
                "arrayJoiner",
                new ArrayJoinerNode
                {
                    NextKey = "fileWriter"
                }
            },
            {
                "fileWriter",
                new FileWriterNode()
            }
        };

        nodes["saltGenerator"].Input["Length"] = DataPinFactory.Instance.Create("Length", new DataPinIntermediateValue("Pins.SaltLength"));
        nodes["ivGenerator"].Input["Length"] = DataPinFactory.Instance.Create("Length", new DataPinIntermediateValue("Pins.IvLength"));
        nodes["deriveKey"].Input["KeyLength"] = DataPinFactory.Instance.Create("KeyLength", new DataPinIntermediateValue("Pins.KeyLength"));
        nodes["deriveKey"].Input["SecurePassword"] = DataPinFactory.Instance.Create("SecurePassword", new DataPinIntermediateValue("Pins.Passphrase"));
        nodes["encode"].Input["PlainText"] = DataPinFactory.Instance.Create("PlainText", new DataPinIntermediateValue("Pins.PlainText"));
        nodes["fileWriter"].Input["FileName"] = DataPinFactory.Instance.Create("FileName", new DataPinIntermediateValue("Pins.OutputFileName"));

        GivenNodeInputPinConnectedToNodeOutputPin(nodes, "deriveKey", "Salt", "saltGenerator", "RandomBytes");
        GivenNodeInputPinConnectedToNodeOutputPin(nodes, "encrypt", "PlainTextBytes", "encode", "EncodedBytes");
        GivenNodeInputPinConnectedToNodeOutputPin(nodes, "encrypt", "Iv", "ivGenerator", "RandomBytes");
        GivenNodeInputPinConnectedToNodeOutputPin(nodes, "encrypt", "Key", "deriveKey", "Key");
        GivenNodeInputPinConnectedToNodeOutputPin(nodes, "arrayJoiner", "Part1", "ivGenerator", "RandomBytes");
        GivenNodeInputPinConnectedToNodeOutputPin(nodes, "arrayJoiner", "Part2", "encrypt", "EncryptedBytes");
        GivenNodeInputPinConnectedToNodeOutputPin(nodes, "arrayJoiner", "Part3", "saltGenerator", "RandomBytes");
        GivenNodeInputPinConnectedToNodeOutputPin(nodes, "fileWriter", "InputData", "arrayJoiner", "JoinedOutput");

        var graph = new Graph.Graph(
            new GraphSettings
            {
                Description = "Default native encryption graph for passchamp.",
                Author = "devoctomy",
                Debug = false
            },
            inputPins,
            null,
            nodes,
            "saltGenerator",
            null,
            null);

        return graph;
    }

    private T GetParameterValue<T>(
        string key,
        params KeyValuePair<string, object>[] parameters)
    {
        var pair = parameters.SingleOrDefault(x => x.Key == key);
        if(pair.Equals(default(KeyValuePair<string, object>)))
        {
            throw new KeyNotFoundException($"The parameter '{key}' could not be found.");
        }

        return (T)pair.Value;
    }

    private void GivenNodeInputPinConnectedToNodeOutputPin(
        Dictionary<string, INode> nodes,
        string nodeAName,
        string pinAName,
        string nodeBName,
        string pinBName)
    {
        var nodeA = nodes[nodeAName];
        var nodeB = nodes[nodeBName];

        var nodeBPinPropInfo = nodeB.OutputPinsProperties[pinBName];
        var nodeBPinOutAttribute = (NodeOutputPinAttribute)Attribute.GetCustomAttribute(nodeBPinPropInfo, typeof(NodeOutputPinAttribute));
        nodeA.Input[pinAName] = nodeB.GetOutput(pinBName, nodeBPinOutAttribute.ValueType);
    }
}
