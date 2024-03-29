﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using devoctomy.Passchamp.Core.Cryptography;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using devoctomy.Passchamp.Core.Graph.Data;
using devoctomy.Passchamp.Core.Graph.IO;
using devoctomy.Passchamp.Core.Graph.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace devoctomy.Passchamp.IntTests.Steps;

[Binding]
public sealed class GraphStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public GraphStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"A new dictionary of nodes")]
    public void GivenANewDictionaryOfNodes()
    {
        var nodes = new Dictionary<string, INode>();
        _scenarioContext.Set(nodes, "Nodes");
    }

    [Given(@"All nodes added to a new graph with a start node named (.*)")]
    public void GivenANewGraphWithAStartNodeNamed(string startNode)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");
        _scenarioContext.Set(
            new Graph(
                null,
                null,
                null,
                nodes,
                startNode,
                null,
                null),
            "Graph");
    }

    [Given(@"RandomByteGeneratorNode named (.*) with a length of (.*) and NextKey of (.*)")]
    public void GivenRandomByteGeneratorNodeNamedWithALengthOf(
        string name,
        int length,
        string nextKey)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

        var node = new RandomByteArrayGeneratorNode
        {
            Length = (IDataPin<int>)DataPinFactory.Instance.Create(
                "Length",
                length),
            NextKey = nextKey,
        };

        nodes.Add(name, node);
    }

    [Given(@"DeriveKeyFromPasswordNode named (.*) with a password of (.*) and key length of (.*) and NextKey of (.*)")]
    [Obsolete("DeriveKeyFromPasswordNode is marked as obsolete and will be removed eventually.")]
    public void GivenDeriveKeyFromPasswordNodeNamedWithAPasswordOfAndKeyLengthOf(
        string name,
        string password,
        int keyLength,
        string nextKey)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

        var node = new DeriveKeyFromPasswordNode
        {
            SecurePassword = (IDataPin<SecureString>)DataPinFactory.Instance.Create(
                "SecurePassword",
                new NetworkCredential(null, password).SecurePassword),
            KeyLength = (IDataPin<int>)DataPinFactory.Instance.Create(
                "KeyLength",
                keyLength),
            NextKey = nextKey,
        };

        nodes.Add(name, node);
    }

    [Given(@"SCryptNode named (.*) with a password of (.*) and NextKey of (.*)")]
    public void GivenSCryptNodeNamedWithAPasswordOfAndKeyLengthOf(
        string name,
        string password,
        string nextKey)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

        var node = new SCryptNode(new SecureStringUnpacker());
        node.SecurePassword = (IDataPin<SecureString>)DataPinFactory.Instance.Create(
                "SecurePassword",
                new NetworkCredential(null, password).SecurePassword);
        node.IterationCount = (IDataPin<int>)DataPinFactory.Instance.Create(
            "IterationCount",
            1024);
        node.BlockSize = (IDataPin<int>)DataPinFactory.Instance.Create(
            "BlockSize",
            8);
        node.ThreadCount = (IDataPin<int>)DataPinFactory.Instance.Create(
            "ThreadCount",
            1);
        node.NextKey = nextKey;

        nodes.Add(name, node);
    }

    [Given(@"Utf8EncoderNode named (.*) with plain text of (.*) and NextKey of (.*)")]
    public void GivenUtfEncoderNodeNamed(
        string name,
        string plainText,
        string nextKey)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

        var node = new Utf8EncoderNode
        {
            PlainText = (IDataPin<string>)DataPinFactory.Instance.Create(
                "PlainText",
                plainText),
            NextKey = nextKey,
        };

        nodes.Add(name, node);
    }

    [Given(@"EncryptNode named (.*) and NextKey of (.*)")]
    public void GivenEncryptNodeNamedAndNextKeyOf(
        string name,
        string nextKey)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

        var node = new EncryptNode
        {
            NextKey = nextKey,
        };

        nodes.Add(name, node);
    }

    [Given(@"ArrayJoinerNode named (.*) and NextKey of (.*)")]
    public void GivenArrayJoinerNodeNamed(
        string name,
        string nextKey)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

        var node = new ArrayJoinerNode
        {
            Part1 = null,
            Part2 = null,
            Part3 = null,
            NextKey = nextKey,
        };

        nodes.Add(name, node);
    }

    [Given(@"FileWriterNode named (.*) with a filename of (.*)")]
    public void GivenFileWriterNodeNamedWithAFilenameOf(
        string name,
        string fileName)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

        var node = new FileWriterNode
        {
            FileName = (IDataPin<string>)DataPinFactory.Instance.Create(
                "FileName",
                fileName),
        };

        nodes.Add(name, node);
    }

    [Given(@"FileReaderNode named (.*) with a filename of (.*) and NextKey of (.*)")]
    public void GivenFileReaderNodeNamedWithAFilenameOf(
        string name,
        string fileName,
        string nextKey)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

        var node = new FileReaderNode
        {
            FileName = (IDataPin<string>)DataPinFactory.Instance.Create(
                "FileName",
                fileName),
            NextKey = nextKey,
        };

        nodes.Add(name, node);
    }

    [Given(@"DataParserNode named (.*) with parser sections of (.*) and NextKey of (.*)")]
    public void GivenDataParserNodeNamed(
        string name,
        string parserSections,
        string nextKey)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

        var allSections = new List<DataParserSection>();
        var sections = parserSections.Split(';');
        foreach (var curSection in sections)
        {
            var curSectionParts = curSection.Split(',');
            var startOffset = curSectionParts[1].StartsWith("~") ? Offset.FromEnd : Offset.Absolute;
            var startValue = int.Parse(curSectionParts[1].TrimStart('~'));
            var endOffset = curSectionParts[2].StartsWith("~") ? Offset.FromEnd : Offset.Absolute;
            var endValue = int.Parse(curSectionParts[2].TrimStart('~'));

            var section = new DataParserSection
            {
                Key = curSectionParts[0],
                Start = new ArrayLocation(startOffset, startValue),
                End = new ArrayLocation(endOffset, endValue),
            };
            allSections.Add(section);
        }

        var node = new DataParserNode
        {
            Sections = (IDataPin<List<DataParserSection>>)DataPinFactory.Instance.Create(
                "Sections",
                allSections),
            NextKey = nextKey,
        };

        nodes.Add(name, node);
    }

    [Given(@"DecryptNode named (.*) and NextKey of (.*)")]
    public void GivenDecryptNodeNamed(
        string name,
        string nextKey)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

        var node = new DecryptNode
        {
            NextKey = nextKey,
        };

        nodes.Add(name, node);
    }

    [Given(@"Utf8DecoderNode named (.*)")]
    public void GivenUtf8DecoderNodeNamed(string name)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

        var node = new Utf8DecoderNode
        {
        };

        nodes.Add(name, node);
    }

    [Given(@"Node (.*) input pin (.*) connected to node (.*) output pin (.*)")]
    public void GivenNodeInputPinConnectedToNodeOutputPin(
        string nodeAName,
        string pinAName,
        string nodeBName,
        string pinBName)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");
        var nodeA = nodes[nodeAName];
        var nodeB = nodes[nodeBName];

        var nodeBPinPropInfo = nodeB.OutputPinsProperties[pinBName];
        var nodeBPinOutAttribute = (NodeOutputPinAttribute)Attribute.GetCustomAttribute(nodeBPinPropInfo, typeof(NodeOutputPinAttribute));
        nodeA.Input[pinAName] = nodeB.GetOutput(pinBName, nodeBPinOutAttribute.ValueType);
    }

    [Given(@"Node (.*) input pin (.*) connected to DataParserNode (.*) section (.*) value")]
    public void GivenNodeInputPinConnectedToDataParserNodeSectionValue(
        string nodeAName,
        string pinAName,
        string dataParserNodeName,
        string sectionKey)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");
        var nodeA = nodes[nodeAName];
        var nodeB = nodes[dataParserNodeName] as DataParserNode;
        nodeA.Input[pinAName] = nodeB.GetSectionValue(sectionKey);
    }

    [When(@"Execute graph")]
    public async Task WhenExecuteGraph()
    {
        var graph = _scenarioContext.Get<Graph>("Graph");
        await graph.ExecuteAsync(CancellationToken.None);
    }

    [Then(@"Output file (.*) created of (.*) bytes in length")]
    public static void ThenOutputFileCreatedOfBytesInLength(
        string fileName,
        int length)
    {
        var file = new FileInfo(fileName);
        Assert.True(file.Exists);
        Assert.Equal(length, file.Length);
    }

    [Then(@"All nodes executed")]
    public void ThenAllNodesExecuted()
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");
        Assert.True(nodes.All(x => x.Value.Executed));
    }

    [Then(@"Node (.*) output pin (.*) equals string (.*)")]
    public void ThenNodeOutputPinEqualsString(
        string nodeName,
        string pinName,
        string stringValue)
    {
        var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");
        var node = nodes[nodeName];
        Assert.Equal(stringValue, node.GetOutput<string>(pinName).Value);
    }
}
