using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using Moq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services.CommandLineParser;

public class DefaultArgumentParserServiceTests
{
    [Fact]
    public void GivenOptionsInstance_AndAllOptions_AndArgumentsString_AndAndAllSetOptions_WhenSetDefaultOption_ThenTrueReturned_AndDefaultOptionSet_AndSetOptionsAdded_AndValueRemovedFromArgumentString()
    {
        // Arrange
        var optionsInstance = new CommandLineTestOptions();
        var mockPropertyValueSetterService = new Mock<IPropertyValueSetterService>();
        var propertyValueSetterService = new PropertyValueSetterService();
        var sut = new DefaultArgumentParserService(mockPropertyValueSetterService.Object);
        var argumentsString = "helloworld -b=true -i=2 -f=5.55 -o=pants -e=Apple";
        var allOptions = new Dictionary<PropertyInfo, CommandLineParserOptionAttribute>();
        AddProperty<CommandLineTestOptions>("StringValue", allOptions);
        AddProperty<CommandLineTestOptions>("BoolValue", allOptions);
        AddProperty<CommandLineTestOptions>("IntValue", allOptions);
        AddProperty<CommandLineTestOptions>("FloatValue", allOptions);
        AddProperty<CommandLineTestOptions>("OptionalStringValue", allOptions);
        AddProperty<CommandLineTestOptions>("OptionalEnumValue", allOptions);
        var allSetOptions = new List<CommandLineParserOptionAttribute>();

        mockPropertyValueSetterService.Setup(x => x.SetPropertyValue(
            It.IsAny<object>(),
            It.IsAny<PropertyInfo>(),
            It.IsAny<string>()))
            .Callback((object o, PropertyInfo p, string s) =>
            {
                propertyValueSetterService.SetPropertyValue(o, p, s);
            })
            .Returns(true);

        // Act
        var result = sut.SetDefaultOption(
            optionsInstance,
            allOptions,
            argumentsString,
            allSetOptions);

        // Assert
        Assert.True(result.Success);
        Assert.Single(allSetOptions);
        Assert.Equal("string", allSetOptions[0].LongName);
        Assert.Equal("helloworld", optionsInstance.StringValue);
        Assert.Equal("-b=true -i=2 -f=5.55 -o=pants -e=Apple", result.UpdatedArgumentsString);
    }

    [Theory]
    /*[InlineData("Apple", true)]
    [InlineData("Orange", true)]
    [InlineData("Pear", true)]*/
    [InlineData("Banana", false)]
    public void GivenOptionsInstance_AndAllOptions_AndArgumentsString_AndDefaultArgument_WhenSetDefaultOption_ThenExpectedResultReturned(
        string argumentsString,
        bool expectedResult)
    {
        // Arrange
        var optionsInstance = new CommandLineTestOptions2();
        var sut = new DefaultArgumentParserService(new PropertyValueSetterService());
        var allOptions = new Dictionary<PropertyInfo, CommandLineParserOptionAttribute>();
        AddProperty<CommandLineTestOptions2>("EnumValue", allOptions);
        var allSetOptions = new List<CommandLineParserOptionAttribute>();

        // Act
        var result = sut.SetDefaultOption(
            optionsInstance,
            allOptions,
            argumentsString,
            allSetOptions);

        // Assert
        Assert.Equal(expectedResult, result.Success);
        Assert.Equal(result.Success ? string.Empty : argumentsString, result.InvalidValue);
    }

    [Fact]
    public void GivenArguments_AndNoOptions_WhenSetDefaultOption_ThenArgumentsParsed_AndFalseReturned()
    {
        // Arrange
        var optionsInstance = new CommandLineTestOptions2();
        var sut = new DefaultArgumentParserService(new PropertyValueSetterService());
        var allOptions = new Dictionary<PropertyInfo, CommandLineParserOptionAttribute>();
        var allSetOptions = new List<CommandLineParserOptionAttribute>();
        var argumentsString = "c:/pop/pop.exe -arg1=1 -arg2=2";

        // Act
        var result = sut.SetDefaultOption(
            optionsInstance,
            allOptions,
            argumentsString,
            allSetOptions);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("-arg1=1 -arg2=2", result.UpdatedArgumentsString);
    }

    private static void AddProperty<T>(
        string name,
        Dictionary<PropertyInfo, CommandLineParserOptionAttribute> options)
    {
        var propertyInfo = typeof(T).GetProperty(name);
        var attribute = propertyInfo.GetCustomAttribute<CommandLineParserOptionAttribute>();
        options.Add(propertyInfo, attribute);
    }
}