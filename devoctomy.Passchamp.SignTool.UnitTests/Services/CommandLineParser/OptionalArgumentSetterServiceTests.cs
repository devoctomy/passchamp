using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using Moq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services.CommandLineParser;

public class OptionalArgumentSetterServiceTests
{
    [Fact]
    public void GivenOptionsInstance_AndAllOptions_WhenSetOptionalValues_ThenOptionalValuesSet()
    {
        // Arrange
        var optionsInstance = new CommandLineTestOptions();
        var mockPropertyValueSetterService = new Mock<IPropertyValueSetterService>();
        var propertyValueSetterService = new PropertyValueSetterService();
        var sut = new OptionalArgumentSetterService(mockPropertyValueSetterService.Object);
        var allOptions = new Dictionary<PropertyInfo, CommandLineParserOptionAttribute>();
        AddProperty("StringValue", allOptions);
        AddProperty("BoolValue", allOptions);
        AddProperty("IntValue", allOptions);
        AddProperty("FloatValue", allOptions);
        AddProperty("OptionalStringValue", allOptions);

        mockPropertyValueSetterService.Setup(x => x.SetPropertyValue(
            It.IsAny<object>(),
            It.IsAny<PropertyInfo>(),
            It.IsAny<string>()))
            .Callback((object o, PropertyInfo p, string s) =>
            {
                propertyValueSetterService.SetPropertyValue(o, p, s);
            });

        // Act
        sut.SetOptionalValues(
            optionsInstance,
            allOptions);

        // Assert
        Assert.Equal("Hello World", optionsInstance.OptionalStringValue);
    }

    private static void AddProperty(
        string name,
        Dictionary<PropertyInfo, CommandLineParserOptionAttribute> options)
    {
        var propertyInfo = typeof(CommandLineTestOptions).GetProperty(name);
        var attribute = propertyInfo.GetCustomAttribute<CommandLineParserOptionAttribute>();
        options.Add(propertyInfo, attribute);
    }
}
