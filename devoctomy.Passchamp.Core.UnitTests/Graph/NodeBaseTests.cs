using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.UnitTests.Graph.Test;
using Moq;
using System;
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

            mockNode.Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>()));
            sut.AttachGraph(mockGraph.Object);

            // Act
            await sut.ExecuteAsync(cancellationTokenSource.Token);

            // Assert
            mockGraph.Verify(x => x.GetNode<INode>(It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNode.Verify(x => x.ExecuteAsync(It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
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

            mockNode.Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>()));
            sut.AttachGraph(mockGraph.Object);

            // Act
            await sut.ExecuteAsync(cancellationTokenSource.Token);

            // Assert
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Never);
            mockNode.Verify(x => x.ExecuteAsync(It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Never);
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
        public void GivenTestNode_WhenGetInputGeneric_ThenInputReturned()
        {
            // Arrange
            var sut = new TestNode
            {
                InputTest = new DataPin<string>("InputTest", "Hello World")
            };

            // Act
            var input = sut.GetInput<string>("InputTest");

            // Assert
            Assert.Equal("Hello World", input.Value);
        }

        [Fact]
        public void GivenTestNode_WhenGetInput_ThenInputReturned()
        {
            // Arrange
            var sut = new TestNode
            {
                InputTest = new DataPin<string>("InputTest", "Hello World")
            };

            // Act
            var input = (IDataPin<string>)sut.GetInput("InputTest", typeof(string));

            // Assert
            Assert.Equal("Hello World", input.Value);
        }

        [Fact]
        public void GivenTestNode_WhenGetOutputGeneric_ThenOutputReturned()
        {
            // Arrange
            var sut = new TestNode
            {
                OutputTest = new DataPin<string>("OutputTest", "Hello World")
            };

            // Act
            var output = sut.GetOutput<string>("OutputTest");

            // Assert
            Assert.Equal("Hello World", output.Value);
        }

        [Fact]
        public void GivenTestNode_WhenGetOutput_ThenOutputReturned()
        {
            // Arrange
            var sut = new TestNode
            {
                OutputTest = new DataPin<string>("OutputTest", "Hello World")
            };

            // Act
            var output = (IDataPin<string>)sut.GetOutput("OutputTest", typeof(string));

            // Assert
            Assert.Equal("Hello World", output.Value);
        }

        [Fact]
        public void GivenTestNode_AndUnknownPinName_AndValidate_WhenPrepareOutputDataPin_ThenKeyNotFoundExceptionThrown()
        {
            // Arrange
            var sut = new TestNode();

            // Act & Assert
            Assert.ThrowsAny<KeyNotFoundException>(() =>
            {
                sut.PrepareOutputDataPin("Pants", typeof(string), true);
            });
        }

        [Fact]
        public void GivenTestNode_AndUnknownPinName_AndNotValidate_WhenPrepareOutputDataPin_ThenKeyNotFoundExceptionNotThrown()
        {
            // Arrange
            var sut = new TestNode();

            // Act & Assert
            sut.PrepareOutputDataPin("Pants", typeof(string), false);
        }

        [Fact]
        public void GivenTestNode_AndUnknownPinName_AndValidate_WhenPrepareInputDataPin_ThenKeyNotFoundExceptionThrown()
        {
            // Arrange
            var sut = new TestNode();

            // Act & Assert
            Assert.ThrowsAny<KeyNotFoundException>(() =>
            {
                sut.PrepareInputDataPin("Pants", typeof(string), true);
            });
        }

        [Fact]
        public void GivenTestNode_AndUnknownPinName_AndNotValidate_WhenPrepareInputDataPin_ThenKeyNotFoundExceptionNotThrown()
        {
            // Arrange
            var sut = new TestNode();

            // Act & Assert
            sut.PrepareInputDataPin("Pants", typeof(string), false);
        }

        [Fact]
        public async Task GivenTestNode_AndBypass_AndExecute_ThenExecutedFalse()
        {
            // Arrange
            var sut = new TestNode
            {
                Bypass = new DataPin<bool>("Bypass", true)
            };
            sut.AttachGraph(Mock.Of<IGraph>());

            // Act
            await sut.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.False(sut.Executed);
        }

        [Fact]
        public async Task GivenTestNode_AndNotBypass_AndExecute_ThenExecutedTrue()
        {
            // Arrange
            var sut = new TestNode
            {
                Bypass = new DataPin<bool>("Bypass", false)
            };
            sut.AttachGraph(Mock.Of<IGraph>());

            // Act
            await sut.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.True(sut.Executed);
        }
        
        [Fact]
        public void GivenNull_WhenAttachGraph_ThenArgumentExceptionThrown()
        {
            // Arrange
            var sut = new NodeBase();

            // Act & Assert
            Assert.ThrowsAny<ArgumentException>(() =>
            {
                sut.AttachGraph(null);
            });
        }

        [Fact]
        public void GivenUnattachedNode_WhenExecuteAsync_ThenInvalidOperationExceptionThrown()
        {
            // Arrange
            var sut = new NodeBase();

            // Act & Assert
            Assert.ThrowsAny<InvalidOperationException>(() =>
            {
                sut.ExecuteAsync(CancellationToken.None).GetAwaiter().GetResult();
            });
        }
    }
}
