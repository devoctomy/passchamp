using devoctomy.Passchamp.Core.Exceptions;
using devoctomy.Passchamp.Maui.Data;
using Microsoft.Maui.Storage;
using Moq;
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
            mockSecureStorage.Setup(x => x.GetAsync(
                It.IsAny<string>())).ReturnsAsync(newValue);

            // Act
            await sut.LoadAsync("id", propertyInfo, config);

            // Assert
            Assert.Equal(newValue, config.TestSetting4);
            mockSecureStorage.Verify(x => x.GetAsync(
                It.Is<string>(y => y == "id.Group.Category.TestSetting4")), Times.Once);
        }
    }
}
