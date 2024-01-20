using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using System;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services.CommandLineParser;

public class HelpMessageFormatterTests
{
    [Theory]
    [InlineData(typeof(CommandLineTestOptions), "Data/CommandLineTestOptionsHelpMessage.txt")]
    [InlineData(typeof(CommandLineTestOptions3), "Data/CommandLineTestOptions3HelpMessage.txt")]
    public void GivenOptionsType_WhenFormat_ThenHelpMessageGenerated(
        Type optionsType,
        string expectedMessagePath)
    {
        // Arrange
        var expected = System.IO.File.ReadAllText(expectedMessagePath);
        var sut = new HelpMessageFormatter();

        // Act
        var result = sut.Format(optionsType);

        // Assert
        result = result.Replace("\r\n", "\n");  // Change encoding to Linux, to fix test on Windows
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GivenOptions_WhenGenericFormat_ThenHelpMessageGenerated()
    {
        // Arrange
        var expected = System.IO.File.ReadAllText("Data/CommandLineTestOptionsHelpMessage.txt");
        var sut = new HelpMessageFormatter();

        // Act
        var result = sut.Format<CommandLineTestOptions>();

        // Assert
        result = result.Replace("\r\n", "\n");  // Change encoding to Linux, to fix test on Windows
        Assert.Equal(expected, result);
    }
}
