using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Cryptography;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Extensions;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Core.Graph.Services.GraphPinPrepFunctions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void GivenServiceCollection_WhenAddPasschampCoreServices_ThenServicesAdded()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var mockSecureSettingStorageService = new Mock<ISecureSettingStorageService>();
            var options = new PasschampCoreServicesOptions
            {
                CloudStorageProviderConfigLoaderServiceOptions = new Core.Cloud.CloudStorageProviderConfigLoaderServiceOptions()
            };

            // Services specific to Maui so let's replace them with mocks
            serviceCollection.AddSingleton(Mock.Of<ISecureSettingStorageService>());

            // Act
            serviceCollection.AddPasschampCoreServices(options);


            // Assert
            var provider = serviceCollection.BuildServiceProvider();
            Assert.NotNull(provider.GetService<IGraphLoaderService>());
            Assert.NotNull(provider.GetService<INodesJsonParserService>());
            Assert.NotNull(provider.GetService<IInputPinsJsonParserService>());
            Assert.NotNull(provider.GetService<IInputPinsJsonParserService>());
            Assert.NotNull(provider.GetService<IDataParserSectionParser>());
            Assert.NotNull(provider.GetService<ISecureStringUnpacker>());
            Assert.NotNull(provider.GetService<IPartialSecureJsonReaderService>());
            Assert.NotNull(provider.GetService<IPartialSecureJsonWriterService>());
            Assert.NotNull(provider.GetService<IIOService>());
            Assert.NotNull(provider.GetService<ICloudStorageProviderConfigLoaderService>());

            var pinPrepFunctions = provider.GetServices<IGraphPinPrepFunction>();
            _ = pinPrepFunctions.SingleOrDefault(x => x.GetType() == typeof(DataParserSectionGetterPinPrepFunction));
        }
    }
}
