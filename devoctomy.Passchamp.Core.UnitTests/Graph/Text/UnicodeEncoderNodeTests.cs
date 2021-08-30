using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Text;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Text
{
    public class UnicodeEncoderNodeTests
    {
        [Fact]
        public async Task GivenEncodedBytes_WhenExecute_ThenPlainTextCorrect_AndNextExecuted()
        {
            // Arrange
            var expectedEncodedBytes = new byte[] { 72, 0, 101, 0, 108, 0, 108, 0, 111, 0 };
            var sut = new UnicodeEncoderNode
            {
                PlainText = (IDataPin<string>)DataPinFactory.Instance.Create(
                    "PlainText",
                    "Hello"),
                NextKey = "hello"
            };
            var mockGraph = new Mock<IGraph>();
            var mockNextNode = new Mock<INode>();
            mockGraph.Setup(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)))
                .Returns(mockNextNode.Object);
            var cancellationTokenSource = new CancellationTokenSource();
            sut.AttachGraph(mockGraph.Object);

            // Act
            await sut.ExecuteAsync(cancellationTokenSource.Token);

            // Assert
            Assert.Equal(expectedEncodedBytes, sut.EncodedBytes.Value);
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNextNode.Verify(x => x.ExecuteAsync(It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
