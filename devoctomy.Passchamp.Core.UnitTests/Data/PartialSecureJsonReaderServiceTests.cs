﻿using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Exceptions;
using devoctomy.Passchamp.Core.UnitTests.Data;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Services;

public class PartialSecureJsonReadersERVICETests
{
    [Fact]
    public async Task GivenInvalidValueType_WhenLoadSecureSettingsAsync_ThenObjectDoesNotImplementIPartiallySecureExceptionThrown()
    {
        // Arrange
        var mockSecureSettingStorageService = new Mock<ISecureSettingStorageService>();
        var sut = new PartialSecureJsonReaderService(mockSecureSettingStorageService.Object);

        var value = new Test3PartialSecureConfigFile
        {
            Id = Guid.NewGuid().ToString(),
            TestSetting3 = "Bob Hoskins"
        };
        var json = JsonConvert.SerializeObject(value);
        var input = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));

        mockSecureSettingStorageService.Setup(x => x.IsApplicable(
            It.Is<PropertyInfo>(y => y.Name == "TestSetting3"))).Returns(true);

        // Act & Assert
        await Assert.ThrowsAnyAsync<ObjectDoesNotImplementIPartiallySecureException>(async () =>
        {
            await sut.LoadAsync<Test3PartialSecureConfigFile>(input);
        });
    }

    [Fact]
    public async Task GivenJson_WhenLoad_ThenUnsecureFieldsParsedFromJson_AndSecureFieldLoadedSecurely()
    {
        // Arrange
        var mockSecureSettingStorageService = new Mock<ISecureSettingStorageService>();
        var sut = new PartialSecureJsonReaderService(mockSecureSettingStorageService.Object);

        var value = new TestPartialSecureConfigFile
        {
            Id = Guid.NewGuid().ToString(),
            TestSetting1 = "Hello World!",
            TestSetting2 = 101
        };
        var json = JsonConvert.SerializeObject(value);
        var input = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));

        mockSecureSettingStorageService.Setup(x => x.IsApplicable(
            It.Is<PropertyInfo>(y => y.Name == "TestSetting3"))).Returns(true);

        // Act
        var result = await sut.LoadAsync<TestPartialSecureConfigFile>(input);

        // Assert
        Assert.Equal(value.TestSetting1, result.TestSetting1);
        Assert.Equal(value.TestSetting2, result.TestSetting2);
        Assert.Null(result.TestSetting3);
        mockSecureSettingStorageService.Verify(x => x.LoadAsync(
            It.Is<string>(y => y == value.Id),
            It.Is<PropertyInfo>(y => y.Name == "TestSetting3"),
            It.Is<object>(y => y.GetType() == typeof(TestPartialSecureConfigFile))), Times.Once);
    }
}
