using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using devoctomy.Passchamp.Core.Graph.Data;
using devoctomy.Passchamp.Core.Graph.Text;
using System.Collections.Generic;
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

        [Given(@"A new graph with a start node named (.*)")]
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

        [Given(@"Utf8EncoderNode named (.*) and NextKey of (.*)")]
        public void GivenUtfEncoderNodeNamed(
            string name,
            string nextKey)
        {
            var nodes = _scenarioContext.Get<Dictionary<string, INode>>("Nodes");

            var node = new Utf8EncoderNode
            {
                PlainText = new DataPin("Hello World!"),
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
                PlainTextBytes = ((Utf8EncoderNode)nodes["encoder"]).EncodedBytes,
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

        [When(@"Execute graph")]
        public void WhenExecuteGraph()
        {

        }

        [Then(@"Something")]
        public void ThenSomething()
        {
        }
    }
}
