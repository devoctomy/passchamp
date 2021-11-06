using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services.CommandLineParser
{
    public class CommandLineParserServiceTests
    {
        [Fact]
        public void GivenNothing_WhenCreateDefaultInstance_ThenDefaultInstanceReturned()
        {
            // Arrange

            // Act
            var instance = CommandLineParserService.CreateDefaultInstance();

            // Assert
            Assert.NotNull(instance);
        }

        [Fact]
        public void GivenMissingArguments_AndOptionsType_WhenParseArgumentsAsOptions_ThenFalseReturned()
        {
            // Arrange
            var mockSingleArgumentParserService = new Mock<ISingleArgumentParserService>();
            var mockDefaultArgumentParserService = new Mock<IDefaultArgumentParserService>();
            var mockArgumentMapperService = new Mock<IArgumentMapperService>();
            var mockOptionalArgumentSetterService = new Mock<IOptionalArgumentSetterService>();
            var sut = new CommandLineParserService(
                mockSingleArgumentParserService.Object,
                mockDefaultArgumentParserService.Object,
                mockArgumentMapperService.Object,
                mockOptionalArgumentSetterService.Object);
            var argumentsString = "hello world";

            // Act
            var success = sut.TryParseArgumentsAsOptions<CommandLineTestOptions>(argumentsString, out var options);

            // Assert
            Assert.False(success);
            mockDefaultArgumentParserService.Verify(x => x.SetDefaultOption(
                It.IsAny<CommandLineTestOptions>(),
                It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>(),
                ref argumentsString,
                It.IsAny<List<CommandLineParserOptionAttribute>>()), Times.Once);
            mockOptionalArgumentSetterService.Verify(x => x.SetOptionalValues(
                It.IsAny<CommandLineTestOptions>(),
                It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>()), Times.Once);
            mockArgumentMapperService.Verify(x => x.MapArguments(
                It.IsAny<CommandLineTestOptions>(),
                It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>(),
                It.IsAny<string>(),
                It.IsAny<List<CommandLineParserOptionAttribute>>()), Times.Once);
        }

        [Fact]
        public void GivenRequiredArguments_AndOptionsType_WhenParseArgumentsAsOptions_ThenTrueReturned_AndOptionsSet()
        {
            // Arrange
            var mockSingleArgumentParserService = new Mock<ISingleArgumentParserService>();
            var mockDefaultArgumentParserService = new Mock<IDefaultArgumentParserService>();
            var mockArgumentMapperService = new Mock<IArgumentMapperService>();
            var mockOptionalArgumentSetterService = new Mock<IOptionalArgumentSetterService>();
            var sut = new CommandLineParserService(
                mockSingleArgumentParserService.Object,
                mockDefaultArgumentParserService.Object,
                mockArgumentMapperService.Object,
                mockOptionalArgumentSetterService.Object);
            var argumentsString = "hello world";
            var allOptions = GetAllOptions<CommandLineTestOptions>();

            mockArgumentMapperService.Setup(x => x.MapArguments(
                It.IsAny<CommandLineTestOptions>(),
                It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>(),
                It.IsAny<string>(),
                It.IsAny<List<CommandLineParserOptionAttribute>>()))
                .Callback((object a, Dictionary<PropertyInfo, CommandLineParserOptionAttribute> b, string c, List<CommandLineParserOptionAttribute> d) =>
                {
                    d.AddRange(allOptions.Values);
                });

            // Act
            var success = sut.TryParseArgumentsAsOptions<CommandLineTestOptions>(argumentsString, out var options);

            // Assert
            Assert.True(success);
            mockDefaultArgumentParserService.Verify(x => x.SetDefaultOption(
                It.IsAny<CommandLineTestOptions>(),
                It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>(),
                ref argumentsString,
                It.IsAny<List<CommandLineParserOptionAttribute>>()), Times.Once);
            mockOptionalArgumentSetterService.Verify(x => x.SetOptionalValues(
                It.IsAny<CommandLineTestOptions>(),
                It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>()), Times.Once);
            mockArgumentMapperService.Verify(x => x.MapArguments(
                It.IsAny<CommandLineTestOptions>(),
                It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>(),
                It.IsAny<string>(),
                It.IsAny<List<CommandLineParserOptionAttribute>>()), Times.Once);
        }

        private Dictionary<PropertyInfo, CommandLineParserOptionAttribute> GetAllOptions<T>()
        {
            var propeties = new Dictionary<PropertyInfo, CommandLineParserOptionAttribute>();
            var allProperties = typeof(T).GetProperties();
            foreach (var curProperty in allProperties)
            {
                var optionAttribute = (CommandLineParserOptionAttribute)curProperty.GetCustomAttributes(typeof(CommandLineParserOptionAttribute), true).FirstOrDefault();
                if (optionAttribute != null)
                {
                    propeties.Add(
                        curProperty,
                        optionAttribute);
                }
            }
            return propeties;
        }
    }
}
