using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Exceptions;
using devoctomy.Passchamp.Core.UnitTests.Data;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Services
{
    public class PartialSecureJsonWriterServiceTests
    {
        [Fact]
        public async Task GivenPartiallySecureObject_WhenSave_ThenSavedOutputShouldNotContainSecureFields_AndSecureFieldSavedSecurely()
        {
            // Arrange
            var mockSecureSettingStorageService = new Mock<ISecureSettingStorageService>();
            var sut = new PartialSecureJsonWriterService(mockSecureSettingStorageService.Object);

            var value = new TestPartialSecureConfigFile
            {
                Id = Guid.NewGuid().ToString(),
                TestSetting1 = "Hello World!",
                TestSetting2 = 101,
                TestSetting3 = "This is a secret!"
            };

            var output = new MemoryStream();

            mockSecureSettingStorageService.Setup(x => x.IsApplicable(
                It.Is<PropertyInfo>(y => y.Name == "TestSetting3"))).Returns(true);

            // Act
            await sut.SaveAsync(value, output);

            // Assert
            var jsonRaw = System.Text.Encoding.UTF8.GetString(output.ToArray());
            var json = JObject.Parse(jsonRaw);
            Assert.True(json.ContainsKey("Id"));
            Assert.True(json.ContainsKey("TestSetting1"));
            Assert.True(json.ContainsKey("TestSetting2"));
            Assert.False(json.ContainsKey("TestSetting3"));
            mockSecureSettingStorageService.Verify(x => x.SaveAsync(
                It.Is<string>(y => y == value.Id),
                It.Is<PropertyInfo>(y => y.Name == "TestSetting3"),
                It.Is<object>(y => y == value)), Times.Once);
        }

        [Fact]
        public async Task GivenPartiallySecureObject_AndMissingJsonIgnoreAttribute_WhenSave_ThenMissingJsonIgnoreAttributeExceptionThrown()
        {
            // Arrange
            var mockSecureSettingStorageService = new Mock<ISecureSettingStorageService>();
            var sut = new PartialSecureJsonWriterService(mockSecureSettingStorageService.Object);

            var value = new Test2PartialSecureConfigFile
            {
                Id = Guid.NewGuid().ToString(),
                TestSetting1 = "Hello World!",
                TestSetting2 = 101,
                TestSetting3 = "This is a secret!"
            };

            var output = new MemoryStream();

            mockSecureSettingStorageService.Setup(x => x.IsApplicable(
                It.Is<PropertyInfo>(y => y.Name == "TestSetting3"))).Returns(true);

            // Act & Assert
            await Assert.ThrowsAnyAsync<MissingJsonIgnoreAttributeException>(async () =>
            {
                await sut.SaveAsync(value, output);
            });
        }

        [Fact]
        public void GivenPartiallySecureObject_WhenRemoveAll_ThenAllSecureSettingsRemoved()
        {
            // Arrange
            var mockSecureSettingStorageService = new Mock<ISecureSettingStorageService>();
            var sut = new PartialSecureJsonWriterService(mockSecureSettingStorageService.Object);

            var value = new Test4PartialSecureConfigFile
            {
                Id = Guid.NewGuid().ToString(),
                TestSetting1 = "Hello World!",
                TestSetting2 = 101,
                TestSetting3 = "This is secret 3",
                TestSetting4 = "This is secret 4"
            };

            mockSecureSettingStorageService.Setup(x => x.IsApplicable(
                It.Is<PropertyInfo>(y => y.Name == "TestSetting3"))).Returns(true);

            mockSecureSettingStorageService.Setup(x => x.IsApplicable(
                It.Is<PropertyInfo>(y => y.Name == "TestSetting4"))).Returns(true);

            // Act
            sut.RemoveAll(value);

            // Assert
            mockSecureSettingStorageService.Verify(x => x.Remove(
                It.Is<string>(y => y == value.Id),
                It.Is<PropertyInfo>(y => y.Name == "TestSetting3")), Times.Once);
            mockSecureSettingStorageService.Verify(x => x.Remove(
                It.Is<string>(y => y == value.Id),
                It.Is<PropertyInfo>(y => y.Name == "TestSetting4")), Times.Once);
        }
    }
}
