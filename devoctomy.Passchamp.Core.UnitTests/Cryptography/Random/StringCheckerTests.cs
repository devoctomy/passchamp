using devoctomy.Passchamp.Core.Cryptography.Random;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Cryptography.Random;

public class StringCheckerTests
{
    [Theory]
    [InlineData("Hello World!", "H", true)]
    [InlineData("Hello World!", "z", false)]
    [InlineData("Hello World!", "!", true)]
    [InlineData("Hello World!", "Hello World!", true)]
    [InlineData("Hello World!", "This_is_a_tEst.", false)]
    public void GivenStringValue_AndChars_WhenContainsAtLeastOneOf_ThenCorrectResultReturned(
        string value,
        string chars,
        bool expectedResult)
    {
        // Arrange
        var sut = new StringChecker();

        // Act
        var result = sut.ContainsAtLeastOneOf(value, chars);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}
