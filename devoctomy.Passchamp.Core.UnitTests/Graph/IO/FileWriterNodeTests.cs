using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.IO;
using Moq;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.IO;

public class FileWriterNodeTests
{
    [Fact]
    public async Task GivenFileName_AndDirectoryNotExists_AndInputData_AndNextKey_WhenExecute_ThenFileWritten_AndNextExecuted()
    {
        // Arrange
        var outputDirName = Guid.NewGuid().ToString();
        var inputData = Guid.NewGuid().ToString(null, System.Globalization.CultureInfo.InvariantCulture).ToUpper(System.Globalization.CultureInfo.InvariantCulture);
        var sut = new FileWriterNode
        {
            FileName = (IDataPin<string>)DataPinFactory.Instance.Create(
                "FileName",
                $"{outputDirName}/TestOutputFile1.txt",
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
        sut.AttachGraph(mockGraph.Object);

        // Act
        await sut.ExecuteAsync(cancellationTokenSource.Token);

        // Assert
        var fileData = await File.ReadAllTextAsync($"{outputDirName}/TestOutputFile1.txt").ConfigureAwait(false);
        Assert.Equal(inputData, fileData);
        mockGraph.Verify(x => x.GetNode<INode>(
            It.Is<string>(x => x == sut.NextKey)), Times.Once);
        mockNextNode.Verify(x => x.ExecuteAsync(It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
    }

    [Fact]
    public async Task GivenFileName_AndDirectoryNotExists_AndNotCreateDirectory_AndInputData_AndNextKey_WhenExecute_ThenDirectoryNotFoundExceptionThrown()
    {
        // Arrange
        var inputData = Guid.NewGuid().ToString(null, System.Globalization.CultureInfo.InvariantCulture).ToUpper(System.Globalization.CultureInfo.InvariantCulture);
        var sut = new FileWriterNode
        {
            FileName = (IDataPin<string>)DataPinFactory.Instance.Create(
                "FileName",
                $"{Guid.NewGuid()}/TestOutputFile1.txt",
                typeof(string)),
            CreateDirectory = (IDataPin<bool>)DataPinFactory.Instance.Create(
                "CreateDirectory",
                false),
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
        sut.AttachGraph(mockGraph.Object);

        // Act & Assert
        await Assert.ThrowsAnyAsync<DirectoryNotFoundException>(async () =>
        {
            await sut.ExecuteAsync(cancellationTokenSource.Token);
        });
    }
}
