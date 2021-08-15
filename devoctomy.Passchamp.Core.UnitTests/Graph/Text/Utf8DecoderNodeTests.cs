using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Text;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Text
{
    public class Utf8DecoderNodeTests
    {
        [Fact]
        public async Task GivenEncodedBytes_WhenExecute_ThenPlainTextCorrect_AndNextExecuted()
        {
            // Arrange
            const string expectedPlainText = "Hello";
            var sut = new Utf8DecoderNode
            {
                EncodedBytes = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                    "EncodedBytes",
                    new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f }),
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
            Assert.Equal(expectedPlainText, sut.PlainText.Value);
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNextNode.Verify(x => x.ExecuteAsync(It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
