using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Maui.UnitTests
{
    public class PartialSecureConfigFileBaseTests
    {
        [Fact]
        public async Task GivenPartialSecureSettingObject_WhenSaveAndLoad_ThenObjectCorrectlySavedAndLoaded()
        {
            // Arrange
            var mockServiceProvier = new Mock<IServiceProvider>();
            var mockSecureSettingStorageService = new Mock<ISecureSettingStorageService>();
            var sut = new TestPartialSecureConfigFile(mockSecureSettingStorageService.Object)
            {
                TestSetting1 = Guid.NewGuid().ToString(),
                TestSetting2 = 101,
                TestSetting3 = "Password123"
            };

            mockServiceProvier.Setup(x => x.GetService(
                It.Is<Type>(y => y == typeof(ISecureSettingStorageService))))
                .Returns(mockSecureSettingStorageService.Object);

            mockSecureSettingStorageService.Setup(x => x.LoadAsync(
                It.Is<string>(y => y == "TestPartialSecureConfigFile"),
                It.Is<PropertyInfo>(y => y.Name == "TestSetting3"),
                It.IsAny<object>()))
                .Callback((string id, PropertyInfo property, object instance) =>
                {
                    var prevValue = (string)property.GetValue(instance);
                    property.SetValue(instance, $"{prevValue}.Test");
                });

            var output = new MemoryStream();

            // Act
            await sut.SaveAsync(output);
            output.Seek(0, SeekOrigin.Begin);
            var result = await PartialSecureConfigFileBase.LoadAsync<TestPartialSecureConfigFile>(
                mockServiceProvier.Object,
                output);

            // Assert
            Assert.Equal("TestPartialSecureConfigFile", result.Id);
            Assert.Equal(sut.TestSetting1, result.TestSetting1);
            Assert.Equal(sut.TestSetting2, result.TestSetting2);
            Assert.Equal(".Test", result.TestSetting3);

            mockSecureSettingStorageService.Verify(x => x.SaveAsync(
                It.Is<string>(y => y == "TestPartialSecureConfigFile"),
                It.Is<PropertyInfo>(y => y.Name == "TestSetting3"),
                It.IsAny<object>()), Times.Once);

            mockSecureSettingStorageService.Verify(x => x.LoadAsync(
                It.Is<string>(y => y == "TestPartialSecureConfigFile"),
                It.Is<PropertyInfo>(y => y.Name == "TestSetting3"),
                It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public async Task GivenPartialSecureSettingObject_WhenSave_ThenJsonWrittenToStreamWithoutSecrets()
        {
            // Arrange
            var mockServiceProvier = new Mock<IServiceProvider>();
            var mockSecureSettingStorageService = new Mock<ISecureSettingStorageService>();
            var sut = new TestPartialSecureConfigFile(mockSecureSettingStorageService.Object)
            {
                TestSetting1 = Guid.NewGuid().ToString(),
                TestSetting2 = 101,
                TestSetting3 = "Password123"
            };

            mockServiceProvier.Setup(x => x.GetService(
                It.Is<Type>(y => y == typeof(ISecureSettingStorageService))))
                .Returns(mockSecureSettingStorageService.Object);

            var output = new MemoryStream();

            // Act
            await sut.SaveAsync(output);


            // Assert
            output.Seek(0, SeekOrigin.Begin);
            var jsonString = System.Text.Encoding.UTF8.GetString(output.ToArray());
            var json = JObject.Parse(jsonString);
            Assert.NotNull(json["TestSetting1"]);
            Assert.NotNull(json["TestSetting2"]);
            Assert.False(json.ContainsKey("TestSetting3"));
            mockSecureSettingStorageService.Verify(x => x.SaveAsync(
                It.Is<string>(y => y == "TestPartialSecureConfigFile"),
                It.Is<PropertyInfo>(y => y.Name == "TestSetting3"),
                It.IsAny<object>()), Times.Once);
        }
    }
}
