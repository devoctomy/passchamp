using devoctomy.Passchamp.SignTool.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services
{
    public class CommandLineParserServiceTests
    {
        [Theory]
        [InlineData(
            "-a=1 --apple=hello -b=\"hello world\"",
            new string[] { "-a=1", "--apple=hello", "-b=\"hello world\"" } )]
        public void GivenArguments_AndRegex_WhenMatch_ThenExpectedMatchesReturned(
            string arguments,
            string[] expectedMatches)
        {
            // Arrange
            var sut = new Regex(CommandLineParserService<object>.Regex);

            // Act
            var matches = sut.Matches(arguments);

            // Assert
            var allMatches = matches.Select(x => x.Value.TrimEnd()).ToList();
            foreach (var curExpected in expectedMatches)
            {
                Assert.Contains(curExpected, allMatches);
            }
        }

        [Theory]
        [InlineData("-s=\"hello world\" -b=true -i=1 -f=1.5", "hello world", true, 1, 1.5f)]
        [InlineData("-s=helloworld -b=false -i=2 -f=5.55", "helloworld", false, 2, 5.55f)]
        public void GivenArguments_AndOptionsType_WhenParseArgumentsAsOptions_ThenOptionsParsedCorrectly(
            string arguments,
            string expectedStringValue,
            bool expectedBoolValue,
            int expectedIntValue,
            float expectedFloatValue)
        {
            // Arrange
            var sut = new CommandLineParserService<CommandLineTestOptions>(new SingleArgumentParser());

            // Act
            var result = sut.ParseArgumentsAsOptions(arguments);

            // Assert
            Assert.Equal(expectedStringValue, result.StringValue);
            Assert.Equal(expectedBoolValue, result.BoolValue);
            Assert.Equal(expectedIntValue, result.IntValue);
            Assert.Equal(expectedFloatValue, result.FloatValue);
        }

        [Fact]
        public void GivenArguments_AndRequiredMissing_WhenParseArgumentsAsOptions_ThenArgumentExceptionThrown_AndMessageContainsParamLongNames()
        {
            // Arrange
            var sut = new CommandLineParserService<CommandLineTestOptions>(new SingleArgumentParser());

            // Act & Assert
            var exception = Assert.ThrowsAny<ArgumentException>(() =>
            {
                var result = sut.ParseArgumentsAsOptions(string.Empty);
            });
            Assert.Contains("string,bool,int,float", exception.Message);
        }

        [Fact]
        public void GivenArguments_AndOmitOptional_WhenParseArgumentsAsOptions_ThenOptionalArgumentsSetToDefaultValue()
        {
            // Arrange
            var arguments = "-s=helloworld -b=false -i=2 -f=5.55";
            var sut = new CommandLineParserService<CommandLineTestOptions>(new SingleArgumentParser());

            // Act
            var result = sut.ParseArgumentsAsOptions(arguments);

            // Assert
            Assert.Equal("Hello World", result.OptionalStringValue);
        }
    }
}
