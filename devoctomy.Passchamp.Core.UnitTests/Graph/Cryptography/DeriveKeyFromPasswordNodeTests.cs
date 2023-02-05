using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using Moq;
using System;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Cryptography;

public class DeriveKeyFromPasswordNodeTests
{
    [Fact]
    [Obsolete("DeriveKeyFromPasswordNode is marked as obsolete and will be removed eventually.")]
    public async Task GivenPassword_AndSalt_AndKeyLength_AndIterationCount_AndNextKey_WhenExecute_ThenKeyDerivedFromPassword_AndNextExecuted()
    {
        // Arrange
        var password = "Hello World";
        var salt = new byte[16];
        var keyLength = 32;
        var iterationCount = 10000;
        var sut = new DeriveKeyFromPasswordNode
        {
            SecurePassword = (IDataPin<SecureString>)DataPinFactory.Instance.Create(
                "SecurePassword",
                new NetworkCredential(null, password).SecurePassword),
            Salt = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                "Salt",
                salt),
            KeyLength = (IDataPin<int>)DataPinFactory.Instance.Create(
                "KeyLength",
                keyLength),
            IterationCount = (IDataPin<int>)DataPinFactory.Instance.Create(
                "IterationCount",
                iterationCount),
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
        var keyBase64 = Convert.ToBase64String(sut.Key.Value);
        Assert.Equal("13L8vFYuEpQEsGqd3ApDr2DNVKhibgj1SdahkfZ+Wjs=", keyBase64);
        mockGraph.Verify(x => x.GetNode<INode>(
            It.Is<string>(x => x == sut.NextKey)), Times.Once);
        mockNextNode.Verify(x => x.ExecuteAsync(It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
    }
}
