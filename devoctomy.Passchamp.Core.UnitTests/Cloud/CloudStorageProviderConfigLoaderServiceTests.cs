using Castle.Core.Configuration;
using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Cloud.AmazonS3;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Exceptions;
using devoctomy.Passchamp.Core.UnitTests.Data;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Cloud;

public class CloudStorageProviderConfigLoaderServiceTests
{
    [Fact]
    public async Task GivenOptions_WhenLoadAsync_ThenConfigRefsLoaded()
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

        mockIOService.Setup(x => x.Exists(
            It.IsAny<string>())).Returns(true);

        mockPartialSecureJsonReaderService.Setup(x => x.LoadAsync<CloudStorageProviderConfig>(
            It.IsAny<Stream>())).ReturnsAsync(new CloudStorageProviderConfig
            {
                DisplayName = "some display name"
            });

        // Act
        await sut.LoadAsync(cancellationTokenSource.Token);

        // Assert
        mockIOService.Verify(x => x.Exists(
            It.Is<string>(y => y == expectedPath)), Times.Once);
        mockIOService.Verify(x => x.CreatePathDirectory(
            It.Is<string>(y => y == expectedPath)), Times.Once);
        Assert.Equal(2, sut.Refs.Count);
        Assert.NotNull(sut.Refs.SingleOrDefault(x => x.Id == "Hello"));
        Assert.NotNull(sut.Refs.SingleOrDefault(x => x.Id == "World"));
        Assert.All(sut.Refs, x => x.DisplayName.Equals("some display name"));

        mockIOService.Verify(x => x.ReadAllTextAsync(
            It.Is<string>(y => y == expectedPath),
            It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        mockIOService.Verify(x => x.OpenRead(
            It.Is<string>(x => x == "/Hello.json")), Times.Once);
        mockIOService.Verify(x => x.OpenRead(
            It.Is<string>(x => x == "/World.json")), Times.Once);
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

        mockIOService.Setup(x => x.Exists(
            It.IsAny<string>())).Returns(true);

        await sut.LoadAsync(cancellationTokenSource.Token);

        // Act & Assert
        mockIOService.Verify(x => x.ReadAllTextAsync(
            It.Is<string>(y => y == expectedPath),
            It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
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
        var expectedConfigPath = $"{options.Path}{options.FileName}";
        var expectedProviderConfigPath = $"{options.Path}Hello.json";
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

        mockIOService.Setup(x => x.Exists(
            It.IsAny<string>())).Returns(true);

        await sut.LoadAsync(cancellationTokenSource.Token);

        // Act
        _ = await sut.UnpackConfigAsync<IPartiallySecure>(
            "Hello",
            cancellationTokenSource.Token);

        // Assert
        mockIOService.Verify(x => x.Exists(
            It.Is<string>(y => y == expectedConfigPath)), Times.Once);
        mockIOService.Verify(x => x.CreatePathDirectory(
            It.Is<string>(y => y == expectedConfigPath)), Times.Once);
        mockIOService.Verify(x => x.OpenRead(
            It.Is<string>(y => y == expectedProviderConfigPath)), Times.Exactly(2));
        mockPartialSecureJsonReaderService.Verify(x => x.LoadAsync<object>(
            It.Is<Stream>(y => y == configStream)), Times.Exactly(2));
    }

    [Fact]
    public async Task GivenConfiguration_WhenAddAsync_ThenConfigSavedSecurely_AndRefsUpdatedAndSaved()
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
        using var configOutputStream = new MemoryStream();
        var config = new TestPartialSecureConfigFile
        {
            Id = Guid.NewGuid().ToString(),
            TestSetting1 = "Hello",
            TestSetting2 = 101,
            TestSetting3 = "This is secret!"
        };
        var expectedConfigPath = $"{options.Path}{config.Id}.json";
        var expectedRefs = new List<CloudStorageProviderConfigRef>
        {
            new CloudStorageProviderConfigRef
            {
                Id = config.Id,
                ProviderServiceTypeId = config.ProviderTypeId
            }
        };
        var expectedRefsPath = $"{options.Path}{options.FileName}";

        mockPartialSecureJsonWriterService.Setup(x => x.SaveAsync(
            It.IsAny<object>(),
            It.IsAny<Stream>()))
            .Callback((object value, Stream stream) =>
            {
                var json = JsonConvert.SerializeObject(value);
                var data = Encoding.UTF8.GetBytes(json);
                stream.WriteAsync(data, 0, data.Length, CancellationToken.None);
            })
            .Returns(Task.CompletedTask);

        mockIOService.Setup(x => x.OpenNewWrite(
            It.IsAny<string>())).Returns(configOutputStream);

        // Act
        await sut.AddAsync(
            config,
            cancellationTokenSource.Token);

        // Assert
        mockIOService.Verify(x => x.OpenNewWrite(
            It.Is<string>(y => y == expectedConfigPath)), Times.Once);
        mockPartialSecureJsonWriterService.Verify(x => x.SaveAsync(
            It.Is<object>(y => y == config),
            It.IsAny<Stream>()), Times.Once);
        Assert.Single(sut.Refs);
        Assert.Contains(sut.Refs, x => x.Id == config.Id);
        mockIOService.Verify(x => x.WriteDataAsync(
            It.Is<string>(y => y == expectedRefsPath),
            It.Is<string>(y => y == JsonConvert.SerializeObject(expectedRefs)),
            It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);

        var outputJson = Encoding.UTF8.GetString(configOutputStream.ToArray());
        var savedConfig = JsonConvert.DeserializeObject<TestPartialSecureConfigFile>(outputJson);
        Assert.Equal(config.Id, savedConfig.Id);
    }

    [Fact]
    public async Task GivenConfiguration_WhenUpdateAsync_ThenConfigurationUpdated()
    {
        // Arrange
        var options = new CloudStorageProviderConfigLoaderServiceOptions
        {
            Path = "path/",
            FileName = "filename.json"
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
        var id = Guid.NewGuid().ToString();
        var config = new List<CloudStorageProviderConfigRef>
        {
            new CloudStorageProviderConfigRef
            {
                Id = id,
                ProviderServiceTypeId = Guid.NewGuid().ToString()
            }
        };
        var update = new AmazonS3CloudStorageProviderConfig
        {
            Id = id,
            AccessId = Guid.NewGuid().ToString(),
            SecretKey = Guid.NewGuid().ToString(),
            DisplayName = Guid.NewGuid().ToString(),
            Bucket = Guid.NewGuid().ToString(),
            Path = Guid.NewGuid().ToString(),
            Region = Guid.NewGuid().ToString(),
        };
        using var configOutputStream = new MemoryStream();
        var expectedConfigPath = $"{options.Path}{update.Id}.json";

        mockIOService.Setup(x => x.Exists(
            It.IsAny<string>()))
            .Returns(true);

        mockIOService.Setup(x => x.ReadAllTextAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(JsonConvert.SerializeObject(config));

        mockPartialSecureJsonWriterService.Setup(x => x.SaveAsync(
            It.IsAny<object>(),
            It.IsAny<Stream>()))
            .Callback((object value, Stream stream) =>
            {
                var json = JsonConvert.SerializeObject(value);
                var data = Encoding.UTF8.GetBytes(json);
                stream.WriteAsync(data, 0, data.Length, CancellationToken.None);
            })
            .Returns(Task.CompletedTask);

        mockIOService.Setup(x => x.OpenNewWrite(
            It.IsAny<string>())).Returns(configOutputStream);

        await sut.LoadAsync(cancellationTokenSource.Token);

        // Act
        await sut.UpdateAsync(
            update,
            cancellationTokenSource.Token);

        // Assert
        Assert.Single(sut.Refs);
        Assert.Contains(sut.Refs, x => x.Id == id);
        mockIOService.Verify(x => x.OpenNewWrite(
            It.Is<string>(y => y == expectedConfigPath)), Times.Once);
    }

    [Fact]
    public async Task GivenId_WhenRemoveAsync_ThenConfigurationRemoved_AndRefsUpdatedAndSaved_AndConfigFileDeleted()
    {
        // Arrange
        var options = new CloudStorageProviderConfigLoaderServiceOptions
        {
            Path = "path/",
            FileName = "filename.json"
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
        var id = Guid.NewGuid().ToString();
        var config = new List<CloudStorageProviderConfigRef>
        {
            new CloudStorageProviderConfigRef
            {
                Id = id,
                ProviderServiceTypeId = Guid.NewGuid().ToString()
            }
        };

        mockIOService.Setup(x => x.Exists(
            It.IsAny<string>()))
            .Returns(true);

        mockIOService.Setup(x => x.ReadAllTextAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(JsonConvert.SerializeObject(config));

        await sut.LoadAsync(cancellationTokenSource.Token);

        // Act
        await sut.RemoveAsync(
            id,
            cancellationTokenSource.Token);

        // Assert
        Assert.Empty(sut.Refs);
        mockIOService.Verify(x => x.WriteDataAsync(
            It.IsAny<string>(),
            It.Is<string>(y => y == "[]"),
            It.IsAny<CancellationToken>()), Times.Once);
        mockIOService.Verify(x => x.Delete(
            It.Is<string>(y => y == $"{options.Path}{id}.json")), Times.Once);
    }
}
