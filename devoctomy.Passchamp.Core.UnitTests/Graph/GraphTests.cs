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
                    nodes,
                    startKey);
            });
        }

        [Fact]
        public async Task GivenGraph_WhenExecute_ThenStartNodeExecuted_AndCancellationTokenUsed()
        {
            // Arrange
            var node1 = new Mock<INode>();
            var nodes = new Dictionary<string, INode>
            {
                { "node1", node1.Object },
                { "node2", Mock.Of<INode>() }
            };
            var startKey = "node1";
            var sut = new Core.Graph.Graph(
                nodes,
                startKey);
            var cancellationTokenSource = new CancellationTokenSource();

            node1.Setup(x => x.Execute(
                It.IsAny<Core.Graph.Graph>(),
                It.IsAny<CancellationToken>()));

            // Act
            await sut.ExecuteAsync(cancellationTokenSource.Token);

            // Assert
            node1.Verify(x => x.Execute(
                It.Is<Core.Graph.Graph>(x => x == sut),
                It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
