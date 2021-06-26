﻿using devoctomy.Passchamp.Core.Graph;
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
            var sut = new DecryptNode()
            {
                Cipher = new DataPin(Convert.FromBase64String("do721rp9jGvQeavPmDw34A==")),
                Iv = new DataPin(iv),
                Key = new DataPin(key),
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
            var plainText = System.Text.Encoding.UTF8.GetString(sut.DecryptedBytes.GetValue<byte[]>());
            Assert.Equal(expectedPlainText, plainText);
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNextNode.Verify(x => x.ExecuteAsync(
                It.Is<IGraph>(x => x == mockGraph.Object),
                It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
        }

    }
}
