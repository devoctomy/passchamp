﻿using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services.CommandLineParser;

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
    public void GivenNullArguments_WhenTryParseArgumentsAsOptions_ThenFalseReturned()
    {
        // Arrange
        var mockDefaultArgumentParserService = new Mock<IDefaultArgumentParserService>();
        var mockArgumentMapperService = new Mock<IArgumentMapperService>();
        var mockOptionalArgumentSetterService = new Mock<IOptionalArgumentSetterService>();
        var sut = new CommandLineParserService(
            mockDefaultArgumentParserService.Object,
            mockArgumentMapperService.Object,
            mockOptionalArgumentSetterService.Object);
        var argumentsString = (string)null;

        // Act
        var success = sut.TryParseArgumentsAsOptions<CommandLineTestOptions>(argumentsString, out var results);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void GivenArguments_AndFailedToSetDefaultArgument_WhenTryParseArgumentsAsOptions_ThenFalseReturned()
    {
        // Arrange
        var mockDefaultArgumentParserService = new Mock<IDefaultArgumentParserService>();
        var mockArgumentMapperService = new Mock<IArgumentMapperService>();
        var mockOptionalArgumentSetterService = new Mock<IOptionalArgumentSetterService>();
        var sut = new CommandLineParserService(
            mockDefaultArgumentParserService.Object,
            mockArgumentMapperService.Object,
            mockOptionalArgumentSetterService.Object);
        var argumentsString = "Hello World";

        mockDefaultArgumentParserService.Setup(x => x.SetDefaultOption(
            It.IsAny<Type>(),
            It.IsAny<object>(),
            It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>(),
            It.IsAny<string>(),
            It.IsAny<List<CommandLineParserOptionAttribute>>())).Returns(new DefaultArgumentParserServiceSetDefaultOptionResult
            {
                Success = false
            });

        // Act
        var success = sut.TryParseArgumentsAsOptions<CommandLineTestOptions>(argumentsString, out var results);

        // Assert
        Assert.False(success);
        Assert.Contains("Failed to set default argument", results.Exception.Message);
    }

    [Fact]
    public void GivenMissingArguments_AndOptionsType_WhenParseArgumentsAsOptions_ThenFalseReturned()
    {
        // Arrange
        var mockDefaultArgumentParserService = new Mock<IDefaultArgumentParserService>();
        var mockArgumentMapperService = new Mock<IArgumentMapperService>();
        var mockOptionalArgumentSetterService = new Mock<IOptionalArgumentSetterService>();
        var sut = new CommandLineParserService(
            mockDefaultArgumentParserService.Object,
            mockArgumentMapperService.Object,
            mockOptionalArgumentSetterService.Object);
        var argumentsString = "hello world";

        mockDefaultArgumentParserService.Setup(x => x.SetDefaultOption(
            It.IsAny<Type>(),
            It.IsAny<object>(),
            It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>(),
            It.IsAny<string>(),
            It.IsAny<List<CommandLineParserOptionAttribute>>()))
            .Returns(new DefaultArgumentParserServiceSetDefaultOptionResult
            {
                Success = true
            });

        // Act
        var success = sut.TryParseArgumentsAsOptions<CommandLineTestOptions>(argumentsString, out var results);

        // Assert
        Assert.False(success);
        mockDefaultArgumentParserService.Verify(x => x.SetDefaultOption(
            It.Is<Type>(y => y == typeof(CommandLineTestOptions)),
            It.IsAny<CommandLineTestOptions>(),
            It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>(),
            It.Is<string>(x => x == argumentsString),
            It.IsAny<List<CommandLineParserOptionAttribute>>()), Times.Once);
        mockOptionalArgumentSetterService.Verify(x => x.SetOptionalValues(
            It.Is<Type>(y => y == typeof(CommandLineTestOptions)),
            It.IsAny<CommandLineTestOptions>(),
            It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>()), Times.Once);
        mockArgumentMapperService.Verify(x => x.MapArguments(
            It.Is<Type>(y => y == typeof(CommandLineTestOptions)),
            It.IsAny<CommandLineTestOptions>(),
            It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>(),
            It.IsAny<string>(),
            It.IsAny<List<CommandLineParserOptionAttribute>>()), Times.Once);
    }

    [Fact]
    public void GivenRequiredArguments_AndOptionsType_WhenParseArgumentsAsOptions_ThenTrueReturned_AndOptionsSet()
    {
        // Arrange
        var mockDefaultArgumentParserService = new Mock<IDefaultArgumentParserService>();
        var mockArgumentMapperService = new Mock<IArgumentMapperService>();
        var mockOptionalArgumentSetterService = new Mock<IOptionalArgumentSetterService>();
        var sut = new CommandLineParserService(
            mockDefaultArgumentParserService.Object,
            mockArgumentMapperService.Object,
            mockOptionalArgumentSetterService.Object);
        var argumentsString = "hello world";
        var allOptions = GetAllOptions<CommandLineTestOptions>();

        mockDefaultArgumentParserService.Setup(x => x.SetDefaultOption(
            It.IsAny<Type>(),
            It.IsAny<object>(),
            It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>(),
            It.IsAny<string>(),
            It.IsAny<List<CommandLineParserOptionAttribute>>()))
            .Returns(new DefaultArgumentParserServiceSetDefaultOptionResult
            {
                Success = true
            });

        mockArgumentMapperService.Setup(x => x.MapArguments(
            It.IsAny<Type>(),
            It.IsAny<object>(),
            It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>(),
            It.IsAny<string>(),
            It.IsAny<List<CommandLineParserOptionAttribute>>()))
            .Callback((
                Type _,
                object _,
                Dictionary<PropertyInfo, CommandLineParserOptionAttribute> _,
                string _,
                List<CommandLineParserOptionAttribute> e) =>
            {
                e.AddRange(allOptions.Values);
            });

        // Act
        var success = sut.TryParseArgumentsAsOptions<CommandLineTestOptions>(argumentsString, out var results);

        // Assert
        Assert.True(success);
        mockDefaultArgumentParserService.Verify(x => x.SetDefaultOption(
            It.Is<Type>(y => y == typeof(CommandLineTestOptions)),
            It.IsAny<CommandLineTestOptions>(),
            It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>(),
            It.Is<string>(x => x == argumentsString),
            It.IsAny<List<CommandLineParserOptionAttribute>>()), Times.Once);
        mockOptionalArgumentSetterService.Verify(x => x.SetOptionalValues(
            It.Is<Type>(y => y == typeof(CommandLineTestOptions)),
            It.IsAny<CommandLineTestOptions>(),
            It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>()), Times.Once);
        mockArgumentMapperService.Verify(x => x.MapArguments(
            It.Is<Type>(y => y == typeof(CommandLineTestOptions)),
            It.IsAny<CommandLineTestOptions>(),
            It.IsAny<Dictionary<PropertyInfo, CommandLineParserOptionAttribute>>(),
            It.IsAny<string>(),
            It.IsAny<List<CommandLineParserOptionAttribute>>()), Times.Once);
    }

    private static Dictionary<PropertyInfo, CommandLineParserOptionAttribute> GetAllOptions<T>()
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
