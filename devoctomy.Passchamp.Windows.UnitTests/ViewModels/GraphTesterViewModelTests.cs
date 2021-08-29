using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Windows.Services;
using devoctomy.Passchamp.Windows.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using Xunit;

namespace devoctomy.Passchamp.Windows.UnitTests.ViewModels
{
    public class GraphTesterViewModelTests
    {
        [Fact]
        public void GivenModel_WhenExecute_ThenGraphExecuted_AndCancellationTokenNoneUsed()
        {
            // Arrange
            var mockGraph = new Mock<IGraph>();
            var model = new Model.GraphTesterModel
            {
                Graph = mockGraph.Object
            };
            var sut = new GraphTesterViewModel(
                null,
                model,
                null,
                null);

            // Act
            sut.Execute.Execute(null);

            // Assert
            mockGraph.Verify(x => x.ExecuteAsync(
                It.Is<CancellationToken>(y => y == CancellationToken.None)), Times.Once);
        }

        [Fact]
        public void GivenModel_WhenGraphBrowse_ThenOpenFileDialogDisplayed_AndFileOpenedWithGraphLoaderService_AndModelUpdated()
        {
            // Arrange
            var mockGraph = new Mock<IGraph>();
            var mockFileDialogService = new Mock<IFileDialogService>();
            var mockGraphLoaderService = new Mock<IGraphLoaderService>();
            var model = new Model.GraphTesterModel();
            var sut = new GraphTesterViewModel(
                Mock.Of<ILogger<GraphTesterViewModel>>(),
                model,
                mockGraphLoaderService.Object,
                mockFileDialogService.Object);
            var fileName = "HelloWorld.json";
            var startNode = "pop";
            var nodes = new System.Collections.Generic.Dictionary<string, INode>()
            {
                { startNode, new NodeBase() }
            };
            var graph = new Graph(null, nodes, startNode, null);

            mockFileDialogService.Setup(x => x.OpenFile(
                It.IsAny<OpenFileDialogOptions>(),
                out fileName)).Returns(true);

            mockGraphLoaderService.Setup(x => x.LoadAsync(
                It.IsAny<string>(),
                It.IsAny<IGraph.GraphOutputMessageDelegate>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(graph);

            // Act
            sut.GraphBrowse.Execute(null);

            // Assert
            mockFileDialogService.Verify(x => x.OpenFile(
                It.IsAny<OpenFileDialogOptions>(),
                out fileName), Times.Once);
            mockGraphLoaderService.Verify(x => x.LoadAsync(
                It.Is<string>(y => y == fileName),
                It.IsAny<IGraph.GraphOutputMessageDelegate>(),
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(graph, model.Graph);
        }
    }
}
