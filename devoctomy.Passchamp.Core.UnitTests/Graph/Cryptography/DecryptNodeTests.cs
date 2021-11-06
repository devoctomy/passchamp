using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Cryptography
{
    public class DecryptNodeTests
    {
        [Fact]
        public async Task GivenCipherText_AndIv_AndKey_AndNextKey_WhenExecute_ThenCipherDecrypted_AndNextExecuted()
        {
            // Arrange
            var expectedPlainText = "Hello World";
            var iv = new byte[16];
            var key = new byte[32];
            var sut = new DecryptNode
            {
                Cipher = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                    "Cipher",
                    Convert.FromBase64String("do721rp9jGvQeavPmDw34A==")),
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
            var plainText = System.Text.Encoding.UTF8.GetString(sut.DecryptedBytes.Value);
            Assert.Equal(expectedPlainText, plainText);
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNextNode.Verify(x => x.ExecuteAsync(It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
        }

    }
}
