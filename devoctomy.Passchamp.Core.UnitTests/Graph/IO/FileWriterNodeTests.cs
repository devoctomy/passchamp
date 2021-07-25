using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.IO;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.IO
{
    public class FileWriterNodeTests
    {
        [Fact]
        public async Task GivenFileName_AndInputData_AndNextKey_WhenExecute_ThenFileWritten_AndNextExecuted()
        {
            // Arrange
            var inputData = Guid.NewGuid().ToString(null, System.Globalization.CultureInfo.InvariantCulture).ToUpper();
            var sut = new FileWriterNode
            {
                FileName = (IDataPin<string>)DataPinFactory.Instance.Create(
                    "FileName",
                    "Data/TestOutputFile1.txt",
                    typeof(string)),
                InputData = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                    "InputData",
                    System.Text.Encoding.UTF8.GetBytes(inputData),
                    typeof(byte[])),
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
            var fileData = await System.IO.File.ReadAllTextAsync("Data/TestOutputFile1.txt").ConfigureAwait(false);
            Assert.Equal(inputData, fileData);
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNextNode.Verify(x => x.ExecuteAsync(
                It.Is<IGraph>(x => x == mockGraph.Object),
                It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
