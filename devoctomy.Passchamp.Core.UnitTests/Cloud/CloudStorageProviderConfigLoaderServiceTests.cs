using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Exceptions;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Cloud
{
    public class CloudStorageProviderConfigLoaderServiceTests
    {
        [Fact]
        public async Task GivenOptions_WhenLoad_ThenConfigRefsLoaded()
        {
            // Arrange
            var options = new CloudStorageProviderConfigLoaderServiceOptions
            {
                Path = "/",
                FileName = "config.json"
            };
            var mockPartialSecureJsonReaderService = new Mock<IPartialSecureJsonReaderService>();
            var mockPartialSecureJsonWriterService = new Mock<IPartialSecureJsonWriterService>();
            var mockIOService = new Mock<IIOService>();
            var sut = new CloudStorageProviderConfigLoaderService(
                options,
                mockPartialSecureJsonReaderService.Object,
                mockPartialSecureJsonWriterService.Object,
                mockIOService.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            var expectedPath = $"{options.Path}{options.FileName}";
            var configRefs = new List<CloudStorageProviderConfigRef>
            {
                new CloudStorageProviderConfigRef
                {
                    Id = "Hello"
                },
                new CloudStorageProviderConfigRef
                {
                    Id = "World"
                }
            };

            mockIOService.Setup(x => x.ReadAllTextAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(JsonConvert.SerializeObject(configRefs));

            // Act
            await sut.LoadAsync(cancellationTokenSource.Token);

            // Assert
            Assert.Equal(2, sut.Refs.Count);
            Assert.NotNull(sut.Refs.SingleOrDefault(x => x.Id == "Hello"));
            Assert.NotNull(sut.Refs.SingleOrDefault(x => x.Id == "World"));
            mockIOService.Verify(x => x.ReadAllTextAsync(
                It.Is<string>(y => y == expectedPath),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenUnknownId_WhenUnpackConfigAsync_ThenUnknownCloudStorageProviderConfigIdExceptionThrown()
        {
            // Arrange
            var options = new CloudStorageProviderConfigLoaderServiceOptions
            {
                Path = "/",
                FileName = "config.json"
            };
            var mockPartialSecureJsonReaderService = new Mock<IPartialSecureJsonReaderService>();
            var mockPartialSecureJsonWriterService = new Mock<IPartialSecureJsonWriterService>();
            var mockIOService = new Mock<IIOService>();
            var sut = new CloudStorageProviderConfigLoaderService(
                options,
                mockPartialSecureJsonReaderService.Object,
                mockPartialSecureJsonWriterService.Object,
                mockIOService.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            var expectedPath = $"{options.Path}{options.FileName}";
            var configRefs = new List<CloudStorageProviderConfigRef>
            {
                new CloudStorageProviderConfigRef
                {
                    Id = "Hello"
                },
                new CloudStorageProviderConfigRef
                {
                    Id = "World"
                }
            };

            mockIOService.Setup(x => x.ReadAllTextAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(JsonConvert.SerializeObject(configRefs));

            await sut.LoadAsync(cancellationTokenSource.Token);

            // Act & Assert
            await Assert.ThrowsAnyAsync<UnknownCloudStorageProviderConfigIdException>(async () =>
            {
                await sut.UnpackConfigAsync<IPartiallySecure>(
                    "Unknown",
                    cancellationTokenSource.Token);
            });
        }

        [Fact]
        public async Task GivenId_WhenUnpackConfigAsync_ThenConfigReferenceStreamLoaded_AndConfigRead()
        {
            // Arrange
            var options = new CloudStorageProviderConfigLoaderServiceOptions
            {
                Path = "/",
                FileName = "config.json"
            };
            var mockPartialSecureJsonReaderService = new Mock<IPartialSecureJsonReaderService>();
            var mockPartialSecureJsonWriterService = new Mock<IPartialSecureJsonWriterService>();
            var mockIOService = new Mock<IIOService>();
            var sut = new CloudStorageProviderConfigLoaderService(
                options,
                mockPartialSecureJsonReaderService.Object,
                mockPartialSecureJsonWriterService.Object,
                mockIOService.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            var expectedPath = $"{options.Path}Hello.json";
            using var configStream = new MemoryStream();
            var configRefs = new List<CloudStorageProviderConfigRef>
            {
                new CloudStorageProviderConfigRef
                {
                    Id = "Hello"
                }
            };

            mockIOService.Setup(x => x.OpenRead(It.IsAny<string>())).Returns(configStream);

            mockIOService.Setup(x => x.ReadAllTextAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(JsonConvert.SerializeObject(configRefs));

            await sut.LoadAsync(cancellationTokenSource.Token);

            // Act
            var result = await sut.UnpackConfigAsync<IPartiallySecure>(
                "Hello",
                cancellationTokenSource.Token);

            // Assert
            mockIOService.Verify(x => x.OpenRead(
                It.Is<string>(y => y == expectedPath)), Times.Once);
            mockPartialSecureJsonReaderService.Verify(x => x.LoadAsync<object>(
                It.Is<Stream>(y => y == configStream)), Times.Once);
        }
    }
}
