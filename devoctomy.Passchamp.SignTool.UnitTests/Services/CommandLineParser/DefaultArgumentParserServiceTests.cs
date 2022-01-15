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
        public void GivenOptionsInstance_AndAllOptions_AndArgumentsString_AndAndAllSetOptions_WhenSetDefaultOption_ThenDefaultOptionSet_AndSetOptionsAdded_AndValueRemovedFromArgumentString()
        {
            // Arrange
            var optionsInstance = new CommandLineTestOptions();
            var mockPropertyValueSetterService = new Mock<IPropertyValueSetterService>();
            var propertyValueSetterService = new PropertyValueSetterService();
            var sut = new DefaultArgumentParserService(mockPropertyValueSetterService.Object);
            var argumentsString = "helloworld -b=true -i=2 -f=5.55 -o=pants -e=Apple";
            var allOptions = new Dictionary<PropertyInfo, CommandLineParserOptionAttribute>();
            AddProperty(typeof(CommandLineTestOptions), "StringValue", allOptions);
            AddProperty(typeof(CommandLineTestOptions), "BoolValue", allOptions);
            AddProperty(typeof(CommandLineTestOptions), "IntValue", allOptions);
            AddProperty(typeof(CommandLineTestOptions), "FloatValue", allOptions);
            AddProperty(typeof(CommandLineTestOptions), "OptionalStringValue", allOptions);
            AddProperty(typeof(CommandLineTestOptions), "OptionalEnumValue", allOptions);
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
            sut.SetDefaultOption(
                optionsInstance,
                allOptions,
                ref argumentsString,
                allSetOptions);

            // Assert
            Assert.Single(allSetOptions);
            Assert.Equal("string", allSetOptions[0].LongName);
            Assert.Equal("helloworld", optionsInstance.StringValue);
            Assert.Equal("-b=true -i=2 -f=5.55 -o=pants -e=Apple", argumentsString);
        }

        private void AddProperty(
            Type type,
            string name,
            Dictionary<PropertyInfo, CommandLineParserOptionAttribute> options)
        {
            var propertyInfo = typeof(CommandLineTestOptions).GetProperty(name);
            var attribute = propertyInfo.GetCustomAttribute<CommandLineParserOptionAttribute>();
            options.Add(propertyInfo, attribute);
        }
    }
}