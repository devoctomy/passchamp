using devoctomy.Passchamp.Core.Graph.IO;
using devoctomy.Passchamp.Core.Graph;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Xunit;
using System.IO;
using System.Text;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.IO;

public class StreamWriterNodeTests
{
    [Fact]
    public async Task GivenMemoryStream_AndInputData_AndNextKey_WhenExecute_ThenDataWritten_AndNextExecuted()
    {
        // Arrange
        var outputDirName = Guid.NewGuid().ToString();
        var inputData = Guid.NewGuid().ToString(null, System.Globalization.CultureInfo.InvariantCulture).ToUpper(System.Globalization.CultureInfo.InvariantCulture);
        var outputStream = new MemoryStream();
        var sut = new StreamWriterNode
        {
            Stream = (IDataPin<Stream>)DataPinFactory.Instance.Create(
                "Stream",
                outputStream,
                typeof(MemoryStream)),
            InputData = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                "InputData",
                Encoding.UTF8.GetBytes(inputData),
                typeof(byte[])),
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
        sut.Stream.Value.Seek(0, SeekOrigin.Begin);
        var outputData = Encoding.UTF8.GetString(outputStream.ToArray());
        Assert.Equal(inputData, outputData);
        mockGraph.Verify(x => x.GetNode<INode>(
            It.Is<string>(x => x == sut.NextKey)), Times.Once);
        mockNextNode.Verify(x => x.ExecuteAsync(It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
    }
}
