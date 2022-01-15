using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services.CommandLineParser
{
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
            AddProperty<CommandLineTestOptions>(typeof(CommandLineTestOptions), "StringValue", allOptions);
            AddProperty<CommandLineTestOptions>(typeof(CommandLineTestOptions), "BoolValue", allOptions);
            AddProperty<CommandLineTestOptions>(typeof(CommandLineTestOptions), "IntValue", allOptions);
            AddProperty<CommandLineTestOptions>(typeof(CommandLineTestOptions), "FloatValue", allOptions);
            AddProperty<CommandLineTestOptions>(typeof(CommandLineTestOptions), "OptionalStringValue", allOptions);
            AddProperty<CommandLineTestOptions>(typeof(CommandLineTestOptions), "OptionalEnumValue", allOptions);
            var allSetOptions = new List<CommandLineParserOptionAttribute>();
            var invalidArgument = string.Empty;

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
            var retval = sut.SetDefaultOption(
                optionsInstance,
                allOptions,
                ref argumentsString,
                allSetOptions,
                ref invalidArgument);

            // Assert
            Assert.True(retval);
            Assert.Single(allSetOptions);
            Assert.Equal("string", allSetOptions[0].LongName);
            Assert.Equal("helloworld", optionsInstance.StringValue);
            Assert.Equal("-b=true -i=2 -f=5.55 -o=pants -e=Apple", argumentsString);
        }

        [Theory]
        [InlineData("Apple", true)]
        [InlineData("Orange", true)]
        [InlineData("Pear", true)]
        [InlineData("Banana", false)]
        public void GivenOptionsInstance_AndAllOptions_AndArgumentsString_AndDefaultArgument_WhenSetDefaultOption_ThenExpectedResultReturned(
            string argumentsString,
            bool expectedResult)
        {
            // Arrange
            var optionsInstance = new CommandLineTestOptions2();
            var sut = new DefaultArgumentParserService(new PropertyValueSetterService());
            var allOptions = new Dictionary<PropertyInfo, CommandLineParserOptionAttribute>();
            AddProperty<CommandLineTestOptions2>(typeof(CommandLineTestOptions2), "EnumValue", allOptions);
            var allSetOptions = new List<CommandLineParserOptionAttribute>();
            var originalArgumentsString = argumentsString;
            var invalidArgument = string.Empty;

            // Act
            var retval = sut.SetDefaultOption(
                optionsInstance,
                allOptions,
                ref argumentsString,
                allSetOptions,
                ref invalidArgument);

            // Assert
            Assert.Equal(expectedResult, retval);
            Assert.Equal(retval ? string.Empty : originalArgumentsString, invalidArgument);
        }

        private void AddProperty<T>(
            Type type,
            string name,
            Dictionary<PropertyInfo, CommandLineParserOptionAttribute> options)
        {
            var propertyInfo = typeof(T).GetProperty(name);
            var attribute = propertyInfo.GetCustomAttribute<CommandLineParserOptionAttribute>();
            options.Add(propertyInfo, attribute);
        }
    }
}