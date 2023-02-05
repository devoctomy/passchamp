using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Data;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Data;

public class ArrayJoinerNodeTests
{
    [Fact]
    public async Task GivenPart1_AndPart2_AndPart3_AndPart4_WhenExecute_ThenPartsJoined_AndNextExecuted()
    {
        // Arrange
        var expectedJoinedArray = new byte[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 };
        var sut = new ArrayJoinerNode
        {
            Part1 = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                "Part1",
                new byte[] { 1, 1, 1, 1 }),
            Part2 = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                "Part2",
                new byte[] { 2, 2, 2, 2 }),
            Part3 = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                "Part3",
                new byte[] { 3, 3, 3, 3 }),
            Part4 = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                "Part4",
                new byte[] { 4, 4, 4, 4 }),
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
        Assert.Equal(expectedJoinedArray, sut.JoinedOutput.Value);
        mockGraph.Verify(x => x.GetNode<INode>(
            It.Is<string>(x => x == sut.NextKey)), Times.Once);
        mockNextNode.Verify(x => x.ExecuteAsync(It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
    }
}
