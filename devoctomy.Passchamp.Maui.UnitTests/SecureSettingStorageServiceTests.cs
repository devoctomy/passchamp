using devoctomy.Passchamp.Maui.Exceptions;
using Microsoft.Maui.Storage;
using Moq;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Maui.UnitTests
{
    public class SecureSettingStorageServiceTests
    {
        [Fact]
        public async Task GivenId_AndPropertyInfo_AndInstance_WhenLoadAsync_ThenGetFromStorage_AndSetProperty()
        {
            // Arrange
            var mockSecureStorage = new Mock<ISecureStorage>();
            var sut = new SecureSettingStorageService(mockSecureStorage.Object);
            var config = new Test2PartialSecureConfigFile(sut)
            {
                TestSetting3 = "Hello World!"
            };
            var propertyInfo = typeof(Test2PartialSecureConfigFile).GetProperty("TestSetting3");
            var newValue = "Bob Hoskins";
            mockSecureStorage.Setup(x => x.GetAsync(
                It.IsAny<string>())).ReturnsAsync(newValue);

            // Act
            await sut.LoadAsync("id", propertyInfo, config);

            // Assert
            Assert.Equal(newValue, config.TestSetting3);
            mockSecureStorage.Verify(x => x.GetAsync(
                It.Is<string>(y => y == "id.Group.Category.TestSetting3")), Times.Once);
        }

        [Fact]
        public async Task GivenId_AndPropertyInfoWithMissingJsonIgnore_AndInstance_WhenLoadAsync_ThenMissingJsonIgnoreAttributeExceptionThrown()
        {
            // Arrange
            var mockSecureStorage = new Mock<ISecureStorage>();
            var sut = new SecureSettingStorageService(mockSecureStorage.Object);
            var config = new Test2PartialSecureConfigFile(sut)
            {
                TestSetting4 = "Hello World!"
            };
            var propertyInfo = typeof(Test2PartialSecureConfigFile).GetProperty("TestSetting4");

            // Act & Assert
            await Assert.ThrowsAnyAsync<MissingJsonIgnoreAttributeException>(async () =>
            {
                await sut.LoadAsync("id", propertyInfo, config);
            });
        }

        [Fact]
        public async Task GivenId_AndPropertyInfo_AndInstance_WhenLoadAsync_ThenSetToStorage()
        {
            // Arrange
            var mockSecureStorage = new Mock<ISecureStorage>();
            var sut = new SecureSettingStorageService(mockSecureStorage.Object);
            var config = new Test2PartialSecureConfigFile(sut)
            {
                TestSetting3 = "Hello World!"
            };
            var propertyInfo = typeof(Test2PartialSecureConfigFile).GetProperty("TestSetting3");

            // Act
            await sut.SaveAsync("id", propertyInfo, config);

            // Assert
            mockSecureStorage.Verify(x => x.SetAsync(
                It.Is<string>(y => y == "id.Group.Category.TestSetting3"),
                It.Is<string>(y => y == config.TestSetting3)), Times.Once);
        }

        [Fact]
        public async Task GivenId_AndPropertyInfoWithMissingJsonIgnore_AndInstance_WhenSaveAsync_ThenMissingJsonIgnoreAttributeExceptionThrown()
        {
            // Arrange
            var mockSecureStorage = new Mock<ISecureStorage>();
            var sut = new SecureSettingStorageService(mockSecureStorage.Object);
            var config = new Test2PartialSecureConfigFile(sut)
            {
                TestSetting4 = "Hello World!"
            };
            var propertyInfo = typeof(Test2PartialSecureConfigFile).GetProperty("TestSetting4");

            // Act & Assert
            await Assert.ThrowsAnyAsync<MissingJsonIgnoreAttributeException>(async () =>
            {
                await sut.SaveAsync("id", propertyInfo, config);
            });
        }
    }
}
