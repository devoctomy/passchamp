using devoctomy.Passchamp.Core.Graph;
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
                nodes,
                startKey);

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
                    nodes,
                    startKey);
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
                nodes,
                startKey);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.ExecuteAsync(cancellationTokenSource.Token);

            // Assert
            Assert.Equal("node1,node2", string.Join(",", sut.ExecutionOrder));
        }
    }
}
