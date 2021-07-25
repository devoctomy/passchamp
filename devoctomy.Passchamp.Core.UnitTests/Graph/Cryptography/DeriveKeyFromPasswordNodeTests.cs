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
        public async Task GivenPassword_AndSalt_AndKeyLength_AndIterationCount_AndNextKey_WhenExecute_ThenKeyDerivedFromPassword_AndNextExecuted()
        {
            // Arrange
            var password = "Hello World";
            var salt = new byte[16];
            var keyLength = 32;
            var iterationCount = 10000;
            var sut = new DeriveKeyFromPasswordNode
            {
                Password = new DataPin("Password", password),
                Salt = new DataPin("Salt", salt),
                KeyLength = new DataPin("KeyLength", keyLength),
                IterationCount = new DataPin("IterationCount", iterationCount),
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
            var keyBase64 = Convert.ToBase64String(sut.Key.GetValue<byte[]>());
            Assert.Equal("13L8vFYuEpQEsGqd3ApDr2DNVKhibgj1SdahkfZ+Wjs=", keyBase64);
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNextNode.Verify(x => x.ExecuteAsync(
                It.Is<IGraph>(x => x == mockGraph.Object),
                It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
