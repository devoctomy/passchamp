using devoctomy.Passchamp.Core.Graph;
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
                nodes,
                startKey,
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
                    nodes,
                    startKey,
                    null,
                    null);
            });
        }

        [Fact]
        public async Task GivenGraph_WhenExecute_ThenExecutionOrderIsCorrect()
        {
            // Arrange
            var node1 = new NodeBase()
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
                nodes,
                startKey,
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
            var node1 = new TestNode()
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
                nodes,
                startKey,
                outputMessageDelegate,
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
    }
}
