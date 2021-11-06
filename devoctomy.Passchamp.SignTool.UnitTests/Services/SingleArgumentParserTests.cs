using devoctomy.Passchamp.SignTool.Services;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services
{
    public class SingleArgumentParserTests
    {
        [Theory]
        [InlineData("a=hello", "a", "hello")]
        [InlineData("a=\"hello world\"", "a", "hello world")]
        public void GivenArgumentString_WhenParse_ThenCorrectArgumentReturned(
            string argumentString,
            string expectedName,
            string expectedValue)
        {
            // Arrange
            var sut = new SingleArgumentParser();

            // Act
            var argument = sut.Parse(argumentString);

            // Assert
            Assert.Equal(expectedName, argument.Name);
            Assert.Equal(expectedValue, argument.Value);
        }
    }
}
