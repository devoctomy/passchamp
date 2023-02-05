using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using Moq;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Cryptography;

public class EncryptNodeTests
{
    [Fact]
    public async Task GivenPlainText_AndIv_AndKey_AndNextKey_WhenExecute_ThenPlainTextEncrypted_AndNextExecuted()
    {
        // Arrange
        var plainText = "Hello World";
        var iv = new byte[16];
        var key = new byte[32];
        var sut = new EncryptNode
        {
            PlainTextBytes = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                "PlainTextBytes",
                Encoding.UTF8.GetBytes(plainText)),
            Iv = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                "Iv",
                iv),
            Key = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                "Key",
                key),
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
        var encryptedBase64 = Convert.ToBase64String(sut.EncryptedBytes.Value);
        Assert.Equal("do721rp9jGvQeavPmDw34A==", encryptedBase64);
        mockGraph.Verify(x => x.GetNode<INode>(
            It.Is<string>(x => x == sut.NextKey)), Times.Once);
        mockNextNode.Verify(x => x.ExecuteAsync(It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
    }

}
