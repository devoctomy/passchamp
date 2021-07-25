using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Cryptography
{
    public class RandomByteArrayGeneratorNodeTests
    {
        [Fact]
        public async Task GivenLength_AndNextKey_WhenExecute3Times_RandomBytesGenerated_AndAllUnique_AndNextExecuted()
        {
            // Arrange
            var length = 32;
            var sut = new RandomByteArrayGeneratorNode
            {
                Length = (IDataPin<int>)DataPinFactory.Instance.Create(
                    "Length",
                    length),
                NextKey = "hello"
            };
            var mockGraph = new Mock<IGraph>();
            var mockNextNode = new Mock<INode>();
            mockGraph.Setup(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)))
                .Returns(mockNextNode.Object);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var resultsList = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                await sut.ExecuteAsync(
                    mockGraph.Object,
                    cancellationTokenSource.Token);
                resultsList.Add(Convert.ToBase64String(sut.RandomBytes.Value));
            }

            // Assert
            Assert.Equal(3, resultsList.Distinct().ToList().Count);
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Exactly(3));
            mockNextNode.Verify(x => x.ExecuteAsync(
                It.Is<IGraph>(x => x == mockGraph.Object),
                It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Exactly(3));
        }
    }
}
