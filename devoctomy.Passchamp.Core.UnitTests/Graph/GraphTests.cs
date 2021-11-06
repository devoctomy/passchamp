using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Core.UnitTests.Graph.Test;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph
{
    public class GraphTests
    {
        [Fact]
        public void GivenNodes_AndStartKey_WhenConstruct_ThenNodesPresent_AndStartKeySet()
        {
            // Arrange
            var nodes = new Dictionary<string, INode>
            {
                { "node1", Mock.Of<INode>() },
                { "node2", Mock.Of<INode>() }
            };
            var startKey = "node1";

            // Act
            var sut = new Core.Graph.Graph(
                null,
                null,
                null,
                nodes,
                startKey,
                null,
                null,
                null);

            // Assert
            Assert.NotNull(sut.GetNode<INode>("node1"));
            Assert.NotNull(sut.GetNode<INode>("node2"));
            Assert.Equal(startKey, sut.StartKey);
        }

        [Fact]
        public void GivenNodes_AndUnknownStartKey_WhenConstruct_ThenKeyNotFoundExceptionThrown()
        {
            // Arrange
            var nodes = new Dictionary<string, INode>
            {
                { "node1", Mock.Of<INode>() },
                { "node2", Mock.Of<INode>() }
            };
            var startKey = "node3";

            // Act & Assert
            Assert.ThrowsAny<KeyNotFoundException>(() =>
            {
                var sut = new Core.Graph.Graph(
                    null,
                    null,
                    null,
                    nodes,
                    startKey,
                    null,
                    null,
                    null);
            });
        }

        [Fact]
        public async Task GivenGraph_WhenExecute_ThenExecutionOrderIsCorrect()
        {
            // Arrange
            var node1 = new NodeBase
            {
                NextKey = "node2"
            };
            var node2 = new NodeBase();
            var nodes = new Dictionary<string, INode>
            {
                { "node1", node1 },
                { "node2", node2 }
            };
            var startKey = "node1";
            var sut = new Core.Graph.Graph(
                null,
                null,
                null,
                nodes,
                startKey,
                null,
                null,
                null);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.ExecuteAsync(cancellationTokenSource.Token);

            // Assert
            Assert.Equal("node1,node2", string.Join(",", sut.ExecutionOrder));
        }

        [Fact]
        public async Task GivenGraph_AndNodes_AndOutputMessageDelegate_WhenExecute_ThenMessagesOutputFromNodes()
        {
            var node1 = new TestNode
            {
                NextKey = "node2"
            };
            var node2 = new TestNode();
            var nodes = new Dictionary<string, INode>
            {
                { "node1", node1 },
                { "node2", node2 }
            };
            var startKey = "node1";
            var nodesOutput = new List<INode>();

            IGraph.GraphOutputMessageDelegate outputMessageDelegate = delegate(INode node, string message)
            {
                if(node != null)
                {
                    nodesOutput.Add(node);
                }
            };

            var sut = new Core.Graph.Graph(
                null,
                null,
                null,
                nodes,
                startKey,
                outputMessageDelegate,
                null,
                null);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.ExecuteAsync(cancellationTokenSource.Token);

            // Assert
            foreach(var curNode in nodes)
            {
                Assert.Contains(curNode.Value, nodesOutput);
            }    
        }

        [Fact]
        public async Task GivenGraph_WithOutputPinMappedToNodeOutput_WhenExecute_ThenOutputPinValueSet()
        {
            // Arrange
            var node1 = new TestNode();
            node1.Output["OutputTest"] = DataPinFactory.Instance.Create(
                "OutputTest",
                "Hello World!");
            var nodes = new Dictionary<string, INode>
            {
                { "node1", node1 },
            };
            var startKey = "node1";
            var outputPins = new Dictionary<string, IPin>
            {
                { "Output", DataPinFactory.Instance.Create("Output", "node1.OutputTest") }
            };
            var sut = new Core.Graph.Graph(
                null,
                null,
                outputPins,
                nodes,
                startKey,
                null,
                null,
                null);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.ExecuteAsync(cancellationTokenSource.Token);

            // Assert
            Assert.Equal("Hello World!", sut.OutputPins["Output"].ObjectValue);
        }

        [Fact]
        public async Task GivenGraph_WithOutputPinMappedToInputPin_WhenExecute_ThenOutputPinValueSet()
        {
            // Arrange
            var nodes = new Dictionary<string, INode>
            {
                { "node1", new TestNode() },
            };
            var startKey = "node1";
            var inputPins = new Dictionary<string, IPin>
            {
                { "Input", DataPinFactory.Instance.Create("Input", "123456") }
            };
            var outputPins = new Dictionary<string, IPin>
            {
                { "Output", DataPinFactory.Instance.Create("Output", "Pins.Input") }
            };
            var sut = new Core.Graph.Graph(
                null,
                inputPins,
                outputPins,
                nodes,
                startKey,
                null,
                null,
                null);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.ExecuteAsync(cancellationTokenSource.Token);

            // Assert
            Assert.Equal(inputPins["Input"].ObjectValue, sut.OutputPins["Output"].ObjectValue);
        }

        [Fact]
        public async Task GivenGraph_WithOutputPinWithOutputFunction_WhenExecute_ThenOutputPinValueSet()
        {
            // Arrange
            var nodes = new Dictionary<string, INode>
            {
                { "node1", new TestNode() },
            };
            var startKey = "node1";
            var outputPins = new Dictionary<string, IPin>
            {
                { "Output", DataPinFactory.Instance.Create("Output", "TestGraphPinOutputFunction.node1.OutputTest") }
            };
            var outputFunctions = new List<IGraphPinOutputFunction>
            {
                new TestGraphPinOutputFunction()
            };
            var sut = new Core.Graph.Graph(
                null,
                null,
                outputPins,
                nodes,
                startKey,
                null,
                null,
                outputFunctions);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.ExecuteAsync(cancellationTokenSource.Token);

            // Assert
            Assert.Equal("Hello World", sut.OutputPins["Output"].ObjectValue);
        }

        [Fact]
        public async Task GivenGraph_WithInputPinWithPrepFunctionMappedToNodeOutputPin_WhenExecute_TheInputPinValueSet()
        {
            // Arrange
            var testNode = new TestNode();
            testNode.Input["InputTest"] = DataPinFactory.Instance.Create("InputTest", new DataPinIntermediateValue("TestGraphPinPrepFunction.node1.OutputTest"));
            testNode.OutputTest.Value = "Hello World";
            var nodes = new Dictionary<string, INode>
            {
                { "node1", testNode },
            };
            var startKey = "node1";
            var prepFunctions = new List<IGraphPinPrepFunction>
            {
                new TestGraphPinPrepFunction()
            };
            var sut = new Core.Graph.Graph(
                null,
                null,
                null,
                nodes,
                startKey,
                null,
                prepFunctions,
                null);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.ExecuteAsync(cancellationTokenSource.Token);

            // Assert
            Assert.Equal("Hello World", testNode.Input["InputTest"].ObjectValue);
        }

        [Fact]
        public async Task GivenGraph_WithInputPinWithPrepFunctionMappedToInputPin_WhenExecute_TheInputPinValueSet()
        {
            // Arrange
            var testNode = new TestNode();
            testNode.Input["InputTest"] = DataPinFactory.Instance.Create("InputTest", new DataPinIntermediateValue("Pins.Test"));
            testNode.OutputTest.Value = "Hello World";
            var nodes = new Dictionary<string, INode>
            {
                { "node1", testNode },
            };
            var startKey = "node1";
            var prepFunctions = new List<IGraphPinPrepFunction>
            {
                new TestGraphPinPrepFunction()
            };
            var inputPins = new Dictionary<string, IPin>
            {
                { "Test", DataPinFactory.Instance.Create("Test", "Hello World") }
            };
            var sut = new Core.Graph.Graph(
                null,
                inputPins,
                null,
                nodes,
                startKey,
                null,
                prepFunctions,
                null);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.ExecuteAsync(cancellationTokenSource.Token);

            // Assert
            Assert.Equal("Hello World", testNode.Input["InputTest"].ObjectValue);
        }
    }
}
