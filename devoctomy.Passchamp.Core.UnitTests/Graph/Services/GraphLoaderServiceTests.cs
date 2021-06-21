using devoctomy.Passchamp.Core.Graph.Services;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Services
{
    public class GraphLoaderServiceTests
    {
        public GraphLoaderServiceTests()
        {
            File.Delete("Output/test.dat");
        }

        // TODO Pass in pin values
        [Theory]
        [InlineData("Data/complexgraph1.json")]
        public async Task GivenJsonDataStream_WhenLoadAsync_ThenGraphLoaded_AndGraphReturned_AndGraphExecutes_AndOutputFileCreated(string fileName)
        {
            // Arrange
            using var jsonDataStream = File.OpenRead(fileName);
            var sut = new GraphLoaderService(
                new PinsJsonParserService(),
                new NodesJsonParserService());

            // Act
            var result = await sut.LoadAsync(
                jsonDataStream,
                CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            await result.ExecuteAsync(CancellationToken.None);
            Assert.Equal("saltgenerator,ivgenerator,derive,encode,encrypt,joiner,writer", string.Join(",", result.ExecutionOrder));
        }
    }
}
