using devoctomy.Passchamp.Core.Cryptography;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Cryptography
{
    public class RandomNumericGeneratorTests
    {
        [Theory]
        [InlineData(0,100)]
        [InlineData(-100, 100)]
        [InlineData(-100, -50)]
        [InlineData(0, 1)]
        [InlineData(0, 0)]
        public void GivenMin_AndMax_WhenGenerateInt_ThenIntGeneratedWithinRange(
            int min,
            int max)
        {
            // Arrange
            var sut = new RandomNumericGenerator();

            // Act
            var result = sut.GenerateInt(min, max);

            // Assert
            Assert.True(result >= min && result <= max);
        }

        [Fact]
        public void GivenNoArgs_WhenGenerateThousandDoubles_ThenAllDoublesDistinct_AndDoublesGeneratedWithinAllowableBounds()
        {
            // Arrange
            var count = 2000;
            var allDoubles = new List<double>();
            var sum = 0.0d;
            var sut = new RandomNumericGenerator();

            // Act
            for(int i = 0; i < count; i++)
            {
                allDoubles.Add(sut.GenerateDouble());
            }
            sum = allDoubles.Average();

            // Assert
            Assert.Equal(count, allDoubles.Distinct().Count());
            Assert.True(sum >= 0.45d && sum <= 0.55d, $"Result is outside allowable bounds.");
        }
    }
}
