using Microsoft.Maui.Storage;
using Moq;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Maui.UnitTests
{
    public class SecureSettingsServiceBaseTests
    {
        [Fact]
        public async Task GivenSettingsService_WhenLoad_ThenSecureStorageUsedToGetAllPublicProperties_AndSettingsParsedCorrectly()
        {
            // Arrange
            var testSetting1 = "Hello World!";
            var testSetting2 = 101;
            var testSetting3 = 10.10f;
            var mockSecureStorageService = new Mock<ISecureStorage>();
            var sut = new TestSecureSettingsService(mockSecureStorageService.Object);

            mockSecureStorageService.Setup(x => x.GetAsync(
                It.Is<string>(y => y == "TestSecureSettingsService.Test.TestSetting1"))).ReturnsAsync(testSetting1);

            mockSecureStorageService.Setup(x => x.GetAsync(
                It.Is<string>(y => y == "TestSecureSettingsService.Test.TestSetting2"))).ReturnsAsync(testSetting2.ToString());

            mockSecureStorageService.Setup(x => x.GetAsync(
                It.Is<string>(y => y == "TestSecureSettingsService.Test.TestSetting3"))).ReturnsAsync(testSetting3.ToString());

            // Act
            await sut.Load();

            // Assert
            mockSecureStorageService.Verify(x => x.GetAsync(
                It.Is<string>(y => y == "TestSecureSettingsService.Test.TestSetting1")), Times.Once);

            mockSecureStorageService.Verify(x => x.GetAsync(
                It.Is<string>(y => y == "TestSecureSettingsService.Test.TestSetting2")), Times.Once);

            mockSecureStorageService.Verify(x => x.GetAsync(
                It.Is<string>(y => y == "TestSecureSettingsService.Test.TestSetting3")), Times.Once);

            Assert.Equal(testSetting1, sut.TestSetting1);
            Assert.Equal(testSetting2, sut.TestSetting2);
            Assert.Equal(testSetting3, sut.TestSetting3);
        }

        [Fact]
        public async Task GivenSettingsService_WhenSave_ThenStorageServiceUsedToSetAllPublicProperties()
        {
            // Arrange
            var mockSecureStorageService = new Mock<ISecureStorage>();
            var sut = new TestSecureSettingsService(mockSecureStorageService.Object)
            {
                TestSetting1 = "Hello World!",
                TestSetting2 = 101,
                TestSetting3 = 10.10f
            };

            // Act
            await sut.Save();

            // Assert
            mockSecureStorageService.Verify(x => x.SetAsync(
                It.Is<string>(y => y == "TestSecureSettingsService.Test.TestSetting1"),
                It.Is<string>(y => y == sut.TestSetting1.ToString())), Times.Once);

            mockSecureStorageService.Verify(x => x.SetAsync(
                It.Is<string>(y => y == "TestSecureSettingsService.Test.TestSetting2"),
                It.Is<string>(y => y == sut.TestSetting2.ToString())), Times.Once);

            mockSecureStorageService.Verify(x => x.SetAsync(
                It.Is<string>(y => y == "TestSecureSettingsService.Test.TestSetting3"),
                It.Is<string>(y => y == sut.TestSetting3.ToString())), Times.Once);
        }
    }
}