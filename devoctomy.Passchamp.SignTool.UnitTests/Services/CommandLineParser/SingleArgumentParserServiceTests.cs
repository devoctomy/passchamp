using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services.CommandLineParser
{
    public class SingleArgumentParserServiceTests
    {
        [Theory]
        [InlineData("a=hello", "a", "hello")]
        [InlineData("a=\"hello world\"", "a", "hello world")]
        [InlineData("", null, null)]
        public void GivenArgumentString_WhenParse_ThenCorrectArgumentReturned(
            string argumentString,
            string expectedName,
            string expectedValue)
        {
            // Arrange
            var sut = new SingleArgumentParserService();

            // Act
            var argument = sut.Parse(argumentString);

            // Assert
            Assert.Equal(expectedName, argument.Name);
            Assert.Equal(expectedValue, argument.Value);
        }
    }
}
