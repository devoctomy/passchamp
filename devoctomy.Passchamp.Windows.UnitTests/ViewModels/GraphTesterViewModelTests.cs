using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Windows.ViewModels;
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
            var sut = new GraphTesterViewModel(
                null,
                null,
                null);
            sut.Model = new Model.GraphTesterModel
            {
                Graph = mockGraph.Object
            };

            // Act
            sut.Execute.Execute(null);

            // Assert
            mockGraph.Verify(x => x.ExecuteAsync(
                It.Is<CancellationToken>(y => y == CancellationToken.None)), Times.Once);
        }
    }
}
