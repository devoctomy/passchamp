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
            var expectedPlainText = "Hello";
            var sut = new Utf8DecoderNode
            {
                EncodedBytes = new DataPin(new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f }),
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
            Assert.Equal(expectedPlainText, sut.PlainText.GetValue<string>());
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNextNode.Verify(x => x.Execute(
                It.Is<IGraph>(x => x == mockGraph.Object),
                It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
