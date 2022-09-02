using devoctomy.Passchamp.Core.Cryptography.Random;
using System;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Cryptography.Random;

public class MemorablePasswordIntSectionGeneratorTests
{
    [Fact]
    public void GivenApplicableToken_WhenIsApplicable_ThenTrueReturned()
    {
        // Arrange
        var sut = new MemorablePasswordIntSectionGenerator(new RandomNumericGenerator());

        // Act
        var result = sut.IsApplicable("int");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GivenNonApplicableToken_WhenIsApplicable_ThenFalseReturned()
    {
        // Arrange
        var sut = new MemorablePasswordIntSectionGenerator(new RandomNumericGenerator());

        // Act
        var result = sut.IsApplicable("bob");

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("0_100", 0, 100)]
    [InlineData("-100_100", -100, 100)]
    [InlineData("-100_-50", -100, -50)]
    public void GivenArguments_WhenGenerate_ThenExpectedIntGenerated(
        string arguments,
        int expectedMin,
        int expectedMax)
    {
        // Arrange
        var sut = new MemorablePasswordIntSectionGenerator(new RandomNumericGenerator());

        // Act
        var result = sut.Generate(null, arguments);

        // Assert
        var resultInt = int.Parse(result);
        Assert.True(resultInt >= expectedMin && resultInt <= expectedMax);
    }

    [Fact]
    public void GivenArguments_AndMinGreaterThanMax_WhenGenerate_ThenArgumentExceptionThrown()
    {
        // Arrange
        var sut = new MemorablePasswordIntSectionGenerator(new RandomNumericGenerator());

        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() =>
        {
            sut.Generate(null, "100_0");
        });
    }
}
