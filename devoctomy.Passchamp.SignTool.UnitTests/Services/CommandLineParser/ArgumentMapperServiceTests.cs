using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services.CommandLineParser
{
    public class ArgumentMapperServiceTests
    {
        [Fact]
        public void GivenOptionsInstance_AndAllOptions_AndArgumentsString_AndAllSetOptions_WhenMapArguments_ThenArgumentsMapped_AndSetOptionsAdded()
        {
            // Arrange
            var optionsInstance = new CommandLineTestOptions();
            var options = new ArgumentMapperOptions();
            var mockSingleArgumentParserService = new Mock<ISingleArgumentParserService>();
            var singleArgumentParserService = new SingleArgumentParserService();
            var mockPropertyValueSetterService = new Mock<IPropertyValueSetterService>();
            var propertyValueSetterService = new PropertyValueSetterService();
            var sut = new ArgumentMapperService(
                options,
                mockSingleArgumentParserService.Object,
                mockPropertyValueSetterService.Object);
            var argumentsString = "-s=helloworld -b=true -i=2 -f=5.55 -o=pants";
            var allOptions = new Dictionary<PropertyInfo, CommandLineParserOptionAttribute>();
            AddProperty(typeof(CommandLineTestOptions), "StringValue", allOptions);
            AddProperty(typeof(CommandLineTestOptions), "BoolValue", allOptions);
            AddProperty(typeof(CommandLineTestOptions), "IntValue", allOptions);
            AddProperty(typeof(CommandLineTestOptions), "FloatValue", allOptions);
            AddProperty(typeof(CommandLineTestOptions), "OptionalStringValue", allOptions);
            var allSetOptions = new List<CommandLineParserOptionAttribute>();

            mockSingleArgumentParserService.Setup(x => x.Parse(
                It.Is<string>(y => y == "s=helloworld")))
                .Returns(singleArgumentParserService.Parse("s=helloworld"));

            mockSingleArgumentParserService.Setup(x => x.Parse(
                It.Is<string>(y => y == "b=true")))
                .Returns(singleArgumentParserService.Parse("b=true"));

            mockSingleArgumentParserService.Setup(x => x.Parse(
                It.Is<string>(y => y == "i=2")))
                .Returns(singleArgumentParserService.Parse("i=2"));

            mockSingleArgumentParserService.Setup(x => x.Parse(
                It.Is<string>(y => y == "f=5.55")))
                .Returns(singleArgumentParserService.Parse("f=5.55"));

            mockSingleArgumentParserService.Setup(x => x.Parse(
                It.Is<string>(y => y == "o=pants")))
                .Returns(singleArgumentParserService.Parse("o=pants"));

            mockPropertyValueSetterService.Setup(x => x.SetPropertyValue(
                It.IsAny<object>(),
                It.IsAny<PropertyInfo>(),
                It.IsAny<string>()))
                .Callback((object o, PropertyInfo p, string s) =>
                {
                    propertyValueSetterService.SetPropertyValue(o, p, s);
                });


            // Act
            sut.MapArguments(
                optionsInstance,
                allOptions,
                argumentsString,
                allSetOptions);

            // Assert
            Assert.Equal(5, allSetOptions.Count);
            foreach(var curOption in allOptions)
            {
                switch(curOption.Key.Name)
                {
                    case "StringValue":
                        {
                            Assert.Equal("helloworld", curOption.Key.GetValue(optionsInstance));
                            break;
                        }

                    case "IntValue":
                        {
                            Assert.Equal(2, curOption.Key.GetValue(optionsInstance));
                            break;
                        }

                    case "BoolValue":
                        {
                            Assert.Equal(true, curOption.Key.GetValue(optionsInstance));
                            break;
                        }

                    case "FloatValue":
                        {
                            Assert.Equal(5.55f, curOption.Key.GetValue(optionsInstance));
                            break;
                        }

                    case "OptionalStringValue":
                        {
                            Assert.Equal("pants", curOption.Key.GetValue(optionsInstance));
                            break;
                        }
                }
            }
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
