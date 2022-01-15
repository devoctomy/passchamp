using devoctomy.Passchamp.Core.Data;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Data
{
    public class WordListLoaderTests
    {
        [Fact]
        public async Task GivenConfig_AndCancellationToken_WhenLoadAllAsync_ThenListsLoaded_AndListsReturned()
        {
            // Arrange
            var config = new WordListLoaderConfig
            {
                Path = "Data/WordLists",
                Pattern = "*.txt"
            };
            var sut = new WordListLoader(config);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.LoadAllAsync(cancellationTokenSource.Token);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(4, result["fruits"].Count);
            Assert.Equal(8, result["colours"].Count);
        }
    }
}
