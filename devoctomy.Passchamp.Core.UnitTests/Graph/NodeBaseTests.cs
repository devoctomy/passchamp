using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.UnitTests.Graph.Test;
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

            mockNode.Setup(x => x.ExecuteAsync(
                It.IsAny<IGraph>(),
                It.IsAny<CancellationToken>()));

            // Act
            await sut.ExecuteAsync(
                mockGraph.Object,
                cancellationTokenSource.Token);

            // Assert
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNode.Verify(x => x.ExecuteAsync(
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

            mockNode.Setup(x => x.ExecuteAsync(
                It.IsAny<IGraph>(),
                It.IsAny<CancellationToken>()));

            // Act
            await sut.ExecuteAsync(
                mockGraph.Object,
                cancellationTokenSource.Token);

            // Assert
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Never);
            mockNode.Verify(x => x.ExecuteAsync(
                It.Is<IGraph>(x => x == mockGraph.Object),
                It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Never);
        }

        [Fact]
        public void GivenNodeBase_AndPinKey_WhenPrepareInputDataPin_ThenInputDataPinPresent()
        {
            // Arrange
            var sut = new NodeBase();
            const string key = "input1";

            // Act
            sut.PrepareInputDataPin(
                key,
                typeof(string),
                false);

            // Assert
            Assert.NotNull(sut.Input[key]);
        }

        [Fact]
        public void GivenNodeBase_WhenAccessUnpreparedInputPin_ThenKeyNotFoundExceptionThrown()
        {
            // Arrange
            var sut = new NodeBase();
            const string key = "input1";

            // Act & Assert
            Assert.ThrowsAny<KeyNotFoundException>(() =>
            {
                _ = sut.Input[key];
            });
        }

        [Fact]
        public void GivenNodeBase_AndPinKey_WhenPrepareOutputDataPin_ThenOutputDataPinPresent()
        {
            // Arrange
            var sut = new NodeBase();
            const string key = "output1";

            // Act
            sut.PrepareOutputDataPin(
                key,
                typeof(string),
                false);

            // Assert
            Assert.NotNull(sut.Output[key]);
        }

        [Fact]
        public void GivenNodeBase_WhenAccessUnpreparedOutputPin_ThenKeyNotFoundExceptionThrown()
        {
            // Arrange
            var sut = new NodeBase();
            const string key = "output1";

            // Act & Assert
            Assert.ThrowsAny<KeyNotFoundException>(() =>
            {
                _ = sut.Output[key];
            });
        }
    
        [Fact]
        public void GivenNodeBase_WhenGetInput_ThenInputReturned()
        {
            // Arrange
            var nodeBase = new TestNode();
            nodeBase.InputTest = new DataPin<string>("InputTest", "Hello World");

            // Act
            var input = nodeBase.GetInput<string>("InputTest");

            // Assert
            Assert.Equal("Hello World", input.Value);
        }

        [Fact]
        public void GivenNodeBase_WhenGetOutput_ThenOutputReturned()
        {
            // Arrange
            var nodeBase = new TestNode();
            nodeBase.OutputTest = new DataPin<string> ("OutputTest", "Hello World");

            // Act
            var output = nodeBase.GetOutput<string>("OutputTest");

            // Assert
            Assert.Equal("Hello World", output.Value);
        }
    }
}
