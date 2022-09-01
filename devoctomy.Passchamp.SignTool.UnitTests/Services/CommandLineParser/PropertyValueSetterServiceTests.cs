using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using System;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services.CommandLineParser;

public class PropertyValueSetterServiceTests
{
    [Theory]
    [InlineData("StringValue", "Bob Hoskins", "Bob Hoskins")]
    [InlineData("IntValue", "199", "199")]
    [InlineData("BoolValue", "True", "True")]
    [InlineData("FloatValue", "1.4", "1.4")]
    [InlineData("OptionalEnumValue", "Apple", "Apple")]
    [InlineData("OptionalEnumValue", "Orange", "Orange")]
    [InlineData("OptionalEnumValue", "Pear", "Pear")]
    [InlineData("OptionalEnumValue", "Banana", "None")]
    public void GivenOptionsInstance_AndProperty_AndValue_WhenSetPropertyValue_ThenPropertyValueSet(
        string propertyName,
        string value,
        string expectedValue)
    {
        // Arrange
        var optionsInstance = new CommandLineTestOptions();
        var property = typeof(CommandLineTestOptions).GetProperty(propertyName);
        var sut = new PropertyValueSetterService();

        // Act
        sut.SetPropertyValue(
            optionsInstance,
            property,
            value);

        // Assert
        Assert.Equal(expectedValue, property.GetValue(optionsInstance).ToString());
    }

    [Fact]
    public void GivenOptionsInstance_AndUnsupportedProperty_WhenSetPropertyValue_ThenNotSupportedExceptionThrown()
    {
        // Arrange
        var optionsInstance = new CommandLineTestBadOptions();
        var property = typeof(CommandLineTestBadOptions).GetProperty("UnsupportedValue");
        var sut = new PropertyValueSetterService();

        // Act & Assert
        Assert.ThrowsAny<NotSupportedException>(() =>
        {
            sut.SetPropertyValue(
                optionsInstance,
                property,
                null);
        });
    }
}
