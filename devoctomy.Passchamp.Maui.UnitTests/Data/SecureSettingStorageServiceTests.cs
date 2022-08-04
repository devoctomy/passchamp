using devoctomy.Passchamp.Maui.Data;
using Microsoft.Maui.Storage;
using Moq;
using System;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Maui.UnitTests.Data
{
    public class SecureSettingStorageServiceTests
    {
        [Fact]
        public void GivenPropertyDecoratedWithSecureSettingAttribute_WhenIsApplicable_ThenTrueReturned()
        {
            // Arrange
            var sut = new SecureSettingStorageService(null);
            var property = typeof(TestPartialSecureConfigFile).GetProperty("TestSetting3");

            // Act
            var result = sut.IsApplicable(property);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GivenPropertyNotDecoratedWithSecureSettingAttribute_WhenIsApplicable_ThenFalseReturned()
        {
            // Arrange
            var sut = new SecureSettingStorageService(null);
            var property = typeof(TestPartialSecureConfigFile).GetProperty("TestSetting1");

            // Act
            var result = sut.IsApplicable(property);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GivenId_AndPropertyInfo_AndInstance_WhenLoadAsync_ThenGetFromStorage_AndSetProperty()
        {
            // Arrange
            var mockSecureStorage = new Mock<ISecureStorage>();
            var sut = new SecureSettingStorageService(mockSecureStorage.Object);
            var config = new Test2PartialSecureConfigFile()
            {
                TestSetting4 = "Hello World!"
            };
            var propertyInfo = typeof(Test2PartialSecureConfigFile).GetProperty("TestSetting4");
            var newValue = "Bob Hoskins";
            var id = Guid.NewGuid().ToString();
            var expectedKey = $"{id}.Group.Category.TestSetting4";
            mockSecureStorage.Setup(x => x.GetAsync(
                It.IsAny<string>())).ReturnsAsync(newValue);

            // Act
            await sut.LoadAsync(id, propertyInfo, config);

            // Assert
            Assert.Equal(newValue, config.TestSetting4);
            mockSecureStorage.Verify(x => x.GetAsync(
                It.Is<string>(y => y == expectedKey)), Times.Once);
        }

        [Fact]
        public async Task GivenId_AndPropertyInfo_AndInstance_WhenSaveAsync_ThenSetToStorageWithCorrectKey()
        {
            // Arrange
            var mockSecureStorage = new Mock<ISecureStorage>();
            var sut = new SecureSettingStorageService(mockSecureStorage.Object);
            var config = new Test2PartialSecureConfigFile()
            {
                TestSetting4 = "Hello World!"
            };
            var id = Guid.NewGuid().ToString();
            var propertyInfo = typeof(Test2PartialSecureConfigFile).GetProperty("TestSetting4");
            var expectedKey = $"{id}.Group.Category.TestSetting4";

            // Act
            await sut.SaveAsync(id, propertyInfo, config);

            // Assert
            mockSecureStorage.Verify(x => x.SetAsync(
                It.Is<string>(y => y == expectedKey),
                It.Is<string>(y => y == config.TestSetting4)), Times.Once);
        }

        [Fact]
        public void GivenId_AndPropertyInfo_WhenRemove_ThenSettingRemovedFromStorageWithCorrectKey()
        {
            // Arrange
            var mockSecureStorage = new Mock<ISecureStorage>();
            var sut = new SecureSettingStorageService(mockSecureStorage.Object);
            var config = new Test2PartialSecureConfigFile()
            {
                TestSetting4 = "Hello World!"
            };
            var id = Guid.NewGuid().ToString();
            var propertyInfo = typeof(Test2PartialSecureConfigFile).GetProperty("TestSetting4");
            var expectedKey = $"{id}.Group.Category.TestSetting4";

            // Act
            sut.Remove(id, propertyInfo);

            // Assert
            mockSecureStorage.Verify(x => x.Remove(
                It.Is<string>(y => y == expectedKey)), Times.Once);
        }

        [Fact]
        public void GivenNoParams_WhenRemoveAll_ThenAllSettingsRemovedFromStorage()
        {
            // Arrange
            var mockSecureStorage = new Mock<ISecureStorage>();
            var sut = new SecureSettingStorageService(mockSecureStorage.Object);

            // Act
            sut.RemoveAll();

            // Assert
            mockSecureStorage.Verify(x => x.RemoveAll(), Times.Once);
        }
    }
}
