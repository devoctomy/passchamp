using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using devoctomy.Passchamp.Core.Graph.Data;
using devoctomy.Passchamp.Core.Graph.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace devoctomy.Passchamp.IntTests.Steps
{
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
            _scenarioContext.Set(new Graph(nodes, startNode), "Graph");
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
                Length = new DataPin(length),
                NextKey = nextKey,
            };

            nodes.Add(name, node);
        }

        [Given(@"DeriveKeyFromPasswordNode named (.*) with a password of (.*) and key length of (.*) and NextKey of (.*)")]
        public void GivenDeriveKeyFromPasswordNodeNamedWithAPasswordOfAndKeyLengthOf(
            string name,
            string password,
            int keyLength,
            string nextKey)
        {
            var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

            var node = new DeriveKeyFromPasswordNode
            {
                Password = new DataPin(password),
                KeyLength = new DataPin(keyLength),
                NextKey = nextKey,
            };

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
                PlainText = new DataPin(plainText),
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

        [Given(@"ArrayJoinerNode named (.*)")]
        public void GivenArrayJoinerNodeNamed(string name)
        {
            var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

            var node = new ArrayJoinerNode
            {
                Part1 = null,
                Part2 = null,
                Part3 = null,
            };

            nodes.Add(name, node);
        }

        [Given(@"node (.*) input pin (.*) connected to node (.*) output pin (.*)")]
        public void GivenNodeInputPinPlainTextBytesConnectedToNodeOutputPinEncodedBytes(
            string nodeAName,
            string pinAName,
            string nodeBName,
            string pinBName)
        {
            var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");
            var nodeA = nodes[nodeAName];
            var nodeB = nodes[nodeBName];
            nodeA.Input[pinAName] = nodeB.GetOutput(pinBName);
        }


        [When(@"Execute graph")]
        public async Task WhenExecuteGraph()
        {
            var graph = _scenarioContext.Get<Graph>("Graph");
            await graph.ExecuteAsync(CancellationToken.None);
        }

        [Then(@"Something")]
        public void ThenSomething()
        {
        }
    }
}
