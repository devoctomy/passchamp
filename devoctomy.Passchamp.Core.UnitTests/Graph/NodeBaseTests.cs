using devoctomy.Passchamp.Core.Graph;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph
{
    public class NodeBaseTests
    {
        [Fact]
        public async Task GivenNodeBase_AndNextKeySet_WhenExecute_ThenNextNodeExecuted_AndCancellationTokenUsed()
        {
            // Arrange
            var mockGraph = new Mock<IGraph>();
            var mockNode = new Mock<INode>();
            var sut = new NodeBase
            {
                NextKey = "hello"
            };
            var cancellationTokenSource = new CancellationTokenSource();

            mockGraph.Setup(x => x.GetNode<INode>(
                It.IsAny<string>()))
                .Returns(mockNode.Object);

            mockNode.Setup(x => x.Execute(
                It.IsAny<IGraph>(),
                It.IsAny<CancellationToken>()));

            // Act
            await sut.Execute(
                mockGraph.Object,
                cancellationTokenSource.Token);

            // Assert
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNode.Verify(x => x.Execute(
                It.Is<IGraph>(x => x == mockGraph.Object),
                It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenNodeBase_AndNoNextKey_WhenExecute_ThenNoMoreNodesExecuted()
        {
            // Arrange
            var mockGraph = new Mock<IGraph>();
            var mockNode = new Mock<INode>();
            var sut = new NodeBase
            { };
            var cancellationTokenSource = new CancellationTokenSource();

            mockGraph.Setup(x => x.GetNode<INode>(
                It.IsAny<string>()))
                .Returns(mockNode.Object);

            mockNode.Setup(x => x.Execute(
                It.IsAny<IGraph>(),
                It.IsAny<CancellationToken>()));

            // Act
            await sut.Execute(
                mockGraph.Object,
                cancellationTokenSource.Token);

            // Assert
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Never);
            mockNode.Verify(x => x.Execute(
                It.Is<IGraph>(x => x == mockGraph.Object),
                It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Never);
        }

        [Fact]
        public void GivenNodeBase_AndPinKey_WhenPrepareInputDataPin_ThenInputDataPinPresent()
        {
            // Arrange
            var sut = new NodeBase();
            var key = "input1";

            // Act
            sut.PrepareInputDataPin(key);

            // Assert
            Assert.NotNull(sut.Input[key]);
        }

        [Fact]
        public void GivenNodeBase_WhenAccessUnpreparedInputPin_ThenKeyNotFoundExceptionThrown()
        {
            // Arrange
            var sut = new NodeBase();
            var key = "input1";

            // Act & Assert
            Assert.ThrowsAny<KeyNotFoundException>(() =>
            {
                var dataPin = sut.Input[key];
            });
        }

        [Fact]
        public void GivenNodeBase_AndPinKey_WhenPrepareOutputDataPin_ThenOutputDataPinPresent()
        {
            // Arrange
            var sut = new NodeBase();
            var key = "output1";

            // Act
            sut.PrepareOutputDataPin(key);

            // Assert
            Assert.NotNull(sut.Output[key]);
        }

        [Fact]
        public void GivenNodeBase_WhenAccessUnpreparedOutputPin_ThenKeyNotFoundExceptionThrown()
        {
            // Arrange
            var sut = new NodeBase();
            var key = "output1";

            // Act & Assert
            Assert.ThrowsAny<KeyNotFoundException>(() =>
            {
                var dataPin = sut.Output[key];
            });
        }
    }
}
