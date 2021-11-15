using devoctomy.Passchamp.Core.Cryptography;
using devoctomy.Passchamp.Core.Cryptography.Random;
using devoctomy.Passchamp.Core.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Cryptography
{
    public class MemorablePasswordGeneratorTests
    {
        [Fact]
        public async Task GivenValidPattern_AndCancellationToken_WhenGenerateAsync_ThenMemorablePasswordReturned_AndAllTokensReplaced()
        {
            // Arrange
            var memorablePasswordGeneratorSectionGenerators = new List<IMemorablePasswordSectionGenerator>
            {
                new MemorablePasswordIntSectionGenerator(new RandomNumericGenerator()),
                new MemorablePasswordWordListSectionGenerator(new RandomNumericGenerator())
            };
            var mockWordListLoader = new Mock<IWordListLoader>();
            var fruitsList = new List<string> { "apple", "orange", "banana", "pear" };
            mockWordListLoader.Setup(x => x.LoadAllAsync(
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dictionary<string, List<string>>
                {
                    { "fruit", fruitsList }
                });
            var sut = new MemorablePasswordGenerator(
                memorablePasswordGeneratorSectionGenerators,
                mockWordListLoader.Object);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.GenerateAsync(
                "{int:0_100}-{wordlist:fruit_lc}",
                cancellationTokenSource.Token);

            // Assert
            var passwordParts = result.Split('-');
            var intPart = int.Parse(passwordParts[0]);
            Assert.True(intPart >= 0 && intPart <= 100);
            Assert.Contains(passwordParts[1], fruitsList);
        }

        [Fact]
        public async Task GivenInvalidPatternWithUnknownWordList_AndCancellationToken_WhenGenerateAsync_ThenArgumentExceptionThrown()
        {
            // Arrange
            var memorablePasswordGeneratorSectionGenerators = new List<IMemorablePasswordSectionGenerator>
            {
                new MemorablePasswordIntSectionGenerator(new RandomNumericGenerator()),
                new MemorablePasswordWordListSectionGenerator(new RandomNumericGenerator())
            };
            var mockWordListLoader = new Mock<IWordListLoader>();
            var fruitsList = new List<string> { "apple", "orange", "banana", "pear" };
            mockWordListLoader.Setup(x => x.LoadAllAsync(
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dictionary<string, List<string>>());
            var sut = new MemorablePasswordGenerator(
                memorablePasswordGeneratorSectionGenerators,
                mockWordListLoader.Object);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act & Assert
            await Assert.ThrowsAnyAsync<ArgumentException>(async () =>
            {
                await sut.GenerateAsync(
                    "{wordlist:fruit_lc}",
                    cancellationTokenSource.Token);
            });
        }

        [Fact]
        public async Task GivenInvalidPatternWithUnknownGenerator_AndCancellationToken_WhenGenerateAsync_ThenArgumentExceptionThrown()
        {
            // Arrange
            var memorablePasswordGeneratorSectionGenerators = new List<IMemorablePasswordSectionGenerator>
            {
                new MemorablePasswordIntSectionGenerator(new RandomNumericGenerator()),
                new MemorablePasswordWordListSectionGenerator(new RandomNumericGenerator())
            };
            var mockWordListLoader = new Mock<IWordListLoader>();
            var fruitsList = new List<string> { "apple", "orange", "banana", "pear" };
            mockWordListLoader.Setup(x => x.LoadAllAsync(
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dictionary<string, List<string>>());
            var sut = new MemorablePasswordGenerator(
                memorablePasswordGeneratorSectionGenerators,
                mockWordListLoader.Object);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act & Assert
            await Assert.ThrowsAnyAsync<ArgumentException>(async () =>
            {
                await sut.GenerateAsync(
                    "{pop:0_100}",
                    cancellationTokenSource.Token);
            });
        }
    }
}
