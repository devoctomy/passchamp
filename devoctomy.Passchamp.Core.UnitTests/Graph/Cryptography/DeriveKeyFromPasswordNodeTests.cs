using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Cryptography
{
    public class DeriveKeyFromPasswordNodeTests
    {
        [Fact]
        public async Task GivenPassword_AndSalt_AndKeyLength_AndNextKey_WhenExecute_ThenKeyDerivedFromPassword_AndNextExecuted()
        {
            // Arrange
            var password = "Hello World";
            var salt = new byte[16];
            var keyLength = 32;
            var sut = new DeriveKeyFromPasswordNode
            {
                Password = new DataPin(password),
                Salt = new DataPin(salt),
                KeyLength = new DataPin(keyLength),
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
            var keyBase64 = Convert.ToBase64String(sut.Key.GetValue<byte[]>());
            Assert.Equal("4r6j4LSQOT/166ahn04oPny3pQxjnMu6sXCTBsi1z2Q=", keyBase64);
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNextNode.Verify(x => x.Execute(
                It.Is<IGraph>(x => x == mockGraph.Object),
                It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
