using devoctomy.Passchamp.Core.Cryptography;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Cryptography
{
    public class SimpleRandomStringGeneratorTests
    {
        [Theory]
        [InlineData("1234567890")]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("-")]
        [InlineData("           ")]
        public void GivenSelectionOfChars_WhenGetRandomCharFromChars_ThenRandomCharReturned_AndCharFromSelection(string chars)
        {
            // Arrange
            var sut = new SimpleRandomStringGenerator(new RandomNumericGenerator());

            // Act
            var result = sut.GetRandomCharFromChars(chars);

            // Assert
            Assert.Contains(result, chars);
        }

        [Theory]
        [InlineData("1234567890", 5)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", 5)]
        [InlineData("-", 5)]
        [InlineData("           ", 5)]
        public void GivenSelectionOfChars_AndLength_WhenGenerateRandomStringFromChars_ThenRandomStringReturnedOfCorrectLength_AndCharsFromSelection(
            string chars,
            int length)
        {
            // Arrange
            var sut = new SimpleRandomStringGenerator(new RandomNumericGenerator());

            // Act
            var result = sut.GenerateRandomStringFromChars(chars, length);

            // Assert
            Assert.Equal(length, result.Length);
            foreach(var curChar in result)
            {
                Assert.Contains(curChar, chars);
            }
        }
    }
}
