using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Text;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Text
{
    public class Utf8EncoderNodeTests
    {
        [Fact]
        public async Task GivenEncodedBytes_WhenExecute_ThenPlainTextCorrect_AndNextExecuted()
        {
            // Arrange
            var expectedEncodedBytes = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f };
            var sut = new Utf8EncoderNode
            {
                PlainText = new DataPin("Hello"),
                NextKey = "hello"
            };
            var mockGraph = new Mock<IGraph>();
            var mockNextNode = new Mock<INode>();
            mockGraph.Setup(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)))
                .Returns(mockNextNode.Object);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Execute(
                mockGraph.Object,
                cancellationTokenSource.Token);

            // Assert
            Assert.Equal(expectedEncodedBytes, sut.EncodedBytes.GetValue<byte[]>());
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNextNode.Verify(x => x.Execute(
                It.Is<IGraph>(x => x == mockGraph.Object),
                It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
