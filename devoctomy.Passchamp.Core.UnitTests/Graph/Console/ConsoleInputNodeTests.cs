using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Console;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Console
{
    public class ConsoleInputNodeTests
    {
        [Fact]
        public async Task GivenPrompt_WhenExecute_ThenLineInput_AndNextExecuted()
        {
            // Arrange
            var mockSystemConsole = new Mock<ISystemConsole>();
            var prompt = "Enter stuff...";
            var inputLine = "Hello World";
            var iv = new byte[16];
            var key = new byte[32];
            var sut = new ConsoleInputNode(mockSystemConsole.Object)
            {
                Prompt = new DataPin(prompt),
                NextKey = "hello"
            };
            var mockGraph = new Mock<IGraph>();
            var mockNextNode = new Mock<INode>();
            mockGraph.Setup(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)))
                .Returns(mockNextNode.Object);
            var cancellationTokenSource = new CancellationTokenSource();

            mockSystemConsole.Setup(x => x.WriteLine(
                It.IsAny<string>()));

            mockSystemConsole.Setup(x => x.ReadLine())
                .Returns(inputLine);

            // Act
            await sut.ExecuteAsync(
                mockGraph.Object,
                cancellationTokenSource.Token);

            // Assert
            mockSystemConsole.Verify(x => x.WriteLine(
                It.Is<string>(y => y == prompt)), Times.Once);
            mockSystemConsole.Verify(x => x.ReadLine(), Times.Once);
            mockSystemConsole.Setup(x => x.ReadLine())
                .Returns(inputLine);
            mockGraph.Verify(x => x.GetNode<INode>(
                It.Is<string>(x => x == sut.NextKey)), Times.Once);
            mockNextNode.Verify(x => x.ExecuteAsync(
                It.Is<IGraph>(x => x == mockGraph.Object),
                It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
