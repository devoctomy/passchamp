using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.IO;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.IO
{
    public class FileReaderNodeTests
    {
        [Fact]
        public async Task GivenFileName_AndNextKey_WhenExecute_ThenFileRead_AndNextExecuted()
        {
            // Arrange
            var expectedData = "Hello World!\r\n12345";
            var sut = new FileReaderNode
            {
                FileName = (IDataPin<string>)DataPinFactory.Instance.Create(
                    "FileName",
                    "Data/TestInputFile1.txt",
                    typeof(string)),
                NextKey = "hello"
            };
            var mockGraph = new Mock<IGraph>();
            var mockNextNode = new Mock<INode>();
            mockGraph.Setup(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)))
                .Returns(mockNextNode.Object);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.ExecuteAsync(
                mockGraph.Object,
                cancellationTokenSource.Token);

            // Assert
            var bytesData = sut.Bytes.Value;
            var textData = System.Text.Encoding.UTF8.GetString(bytesData);
            Assert.True(string.Compare(expectedData, textData, true, System.Globalization.CultureInfo.InvariantCulture) == 0);
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNextNode.Verify(x => x.ExecuteAsync(
                It.Is<IGraph>(x => x == mockGraph.Object),
                It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
