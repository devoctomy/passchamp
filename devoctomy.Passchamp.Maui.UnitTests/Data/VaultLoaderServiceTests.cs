using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Maui.Data;
using devoctomy.Passchamp.Maui.Exceptions;
using devoctomy.Passchamp.Maui.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Maui.UnitTests.Data;

public class VaultLoaderServiceTests
{
    [Fact]
    public async Task GivenOptions_WhenLoadAsync_ThenPathCreated_AndFileExists_AndJsonReadFromCorrectPath_AndVaultsDeserialisedCorrectly()
    {
        // Arrange
        var mockIoService = new Mock<IIOService>();
        var options = new VaultLoaderServiceOptions
        {
            Path = "folder1/folder2/",
            FileName = "somefile.json"
        };
        var sut = new VaultLoaderService(
            options,
            mockIoService.Object);

        var expectedVaults = new List<VaultIndex>
        {
            new VaultIndex
            {
                Id = Guid.NewGuid().ToString()
            },
            new VaultIndex
            {
                Id = Guid.NewGuid().ToString()
            }
        };

        var cancellationTokenSource = new CancellationTokenSource();

        mockIoService.Setup(x => x.CreatePathDirectory(
            It.IsAny<string>()));

        mockIoService.Setup(x => x.Exists(
            It.IsAny<string>()))
            .Returns(true);

        mockIoService.Setup(x => x.ReadAllTextAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(JsonConvert.SerializeObject(expectedVaults));

        // Act
        await sut.LoadAsync(cancellationTokenSource.Token);

        // Assert
        var expectedPath = $"{options.Path}{options.FileName}";
        mockIoService.Verify(x => x.CreatePathDirectory(
            It.Is<string>(y => y == expectedPath)), Times.Once);
        mockIoService.Setup(x => x.Exists(
            It.Is<string>(y => y == expectedPath)))
            .Returns(true);
        mockIoService.Verify(x => x.ReadAllTextAsync(
            It.Is<string>(y => y == expectedPath),
            It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        Assert.Equal(expectedVaults.Count, sut.Vaults.Count);
        Assert.Contains(sut.Vaults, x => x.Id == expectedVaults[0].Id);
        Assert.Contains(sut.Vaults, x => x.Id == expectedVaults[1].Id);
    }

    [Fact]
    public async Task GivenOptions_WhenLoadAsync_ThenPathCreated_AndFileNotExists_AndNothingLoaded()
    {
        // Arrange
        var mockIoService = new Mock<IIOService>();
        var options = new VaultLoaderServiceOptions
        {
            Path = "folder1/folder2/",
            FileName = "somefile.json"
        };
        var sut = new VaultLoaderService(
            options,
            mockIoService.Object);

        var cancellationTokenSource = new CancellationTokenSource();

        mockIoService.Setup(x => x.Exists(
            It.IsAny<string>()))
            .Returns(false);

        // Act
        await sut.LoadAsync(cancellationTokenSource.Token);

        // Assert
        var expectedPath = $"{options.Path}{options.FileName}";
        mockIoService.Verify(x => x.CreatePathDirectory(
            It.Is<string>(y => y == expectedPath)), Times.Once);
        mockIoService.Setup(x => x.Exists(
            It.Is<string>(y => y == expectedPath)))
            .Returns(true);
        mockIoService.Verify(x => x.ReadAllTextAsync(
            It.Is<string>(y => y == expectedPath),
            It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Never);
        Assert.Empty(sut.Vaults);
    }

    [Fact]
    public async Task GivenCloudStorageProviderConfigRef_AndCloudProviderPath_WhenAddFromCloudProviderAsync_ThenPathCreated_AndJsonWrittenToCorrectPath()
    {
        // Arrange
        var mockIoService = new Mock<IIOService>();
        var options = new VaultLoaderServiceOptions
        {
            Path = "folder1/folder2/",
            FileName = "somefile.json"
        };
        var sut = new VaultLoaderService(
            options,
            mockIoService.Object);

        var cloudStorageProviderConfigRef = new CloudStorageProviderConfigRef
        {
            Id = Guid.NewGuid().ToString(),
            ProviderServiceTypeId = Guid.NewGuid().ToString()
        };
        var cloudProviderPath = "somedir/somefile";

        var expectedVaults = new List<VaultIndex>
        {
            new VaultIndex
            {
                Name = "Not yet decrypted",
                Description = "Not yet decrypted",
                CloudProviderId = cloudStorageProviderConfigRef.ProviderServiceTypeId,
                CloudProviderPath = cloudProviderPath
            }
        };

        var cancellationTokenSource = new CancellationTokenSource();

        // Act
        await sut.AddFromCloudProviderAsync(
            cloudStorageProviderConfigRef,
            cloudProviderPath,
            cancellationTokenSource.Token);

        // Assert
        var expectedPath = $"{options.Path}{options.FileName}";
        mockIoService.Verify(x => x.CreatePathDirectory(
            It.Is<string>(y => y == expectedPath)), Times.Once);
        mockIoService.Verify(x => x.WriteDataAsync(
            It.Is<string>(y => y == expectedPath),
            It.Is<string>(y => CheckVaultJson(expectedVaults, y, false)),
            It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
    }

    [Fact]
    public async Task GivenVaultIndexId_AndVaultIndexExists_WhenRemoveAsync_ThenVaultIndexRemoved_AndPathCreated_AndJsonWrittenToCorrectPath()
    {
        // Arrange
        var mockIoService = new Mock<IIOService>();
        var options = new VaultLoaderServiceOptions
        {
            Path = "folder1/folder2/",
            FileName = "somefile.json"
        };
        var expectedVaults = new List<VaultIndex>
        {
            new VaultIndex
            {
                Id = Guid.NewGuid().ToString()
            },
            new VaultIndex
            {
                Id = Guid.NewGuid().ToString()
            }
        };
        var sut = new VaultLoaderService(
            options,
            mockIoService.Object,
            expectedVaults);

        var cancellationTokenSource = new CancellationTokenSource();

        // Act
        await sut.RemoveAsync(
            expectedVaults[0],
            cancellationTokenSource.Token);

        // Assert
        var expectedPath = $"{options.Path}{options.FileName}";
        mockIoService.Verify(x => x.CreatePathDirectory(
            It.Is<string>(y => y == expectedPath)), Times.Once);
        mockIoService.Verify(x => x.WriteDataAsync(
            It.Is<string>(y => y == expectedPath),
            It.Is<string>(y => CheckVaultJson(expectedVaults, y, false)),
            It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        Assert.Single(sut.Vaults);
    }

    [Fact]
    public async Task GivenVaultIndexId_AndVaultIndexNotExists_WhenRemoveAsync_ThenVaultIndexNotFoundExceptionThrown()
    {
        // Arrange
        var mockIoService = new Mock<IIOService>();
        var options = new VaultLoaderServiceOptions
        {
            Path = "folder1/folder2/",
            FileName = "somefile.json"
        };
        var expectedVaults = new List<VaultIndex>
        {
            new VaultIndex
            {
                Id = Guid.NewGuid().ToString()
            },
            new VaultIndex
            {
                Id = Guid.NewGuid().ToString()
            }
        };
        var sut = new VaultLoaderService(
            options,
            mockIoService.Object,
            expectedVaults);

        var cancellationTokenSource = new CancellationTokenSource();

        // Act & Assert
        await Assert.ThrowsAnyAsync<VaultIndexNotFoundException>(async () =>
        {
            await sut.RemoveAsync(
                new VaultIndex(),
                cancellationTokenSource.Token);
        });
    }

    private static bool CheckVaultJson(
        List<VaultIndex> expectedVaults,
        string rawJson,
        bool validateId)
    {
        var actualVaults = JsonConvert.DeserializeObject<List<VaultIndex>>(rawJson);
        foreach(var curExpectedVault in expectedVaults)
        {
            var matchedVault = default(VaultIndex);
            if(validateId)
            {
                matchedVault = actualVaults.SingleOrDefault(x =>
                x.Id == curExpectedVault.Id &&
                x.CloudProviderId == curExpectedVault.CloudProviderId &&
                x.CloudProviderPath == curExpectedVault.CloudProviderPath &&
                x.Name == curExpectedVault.Name &&
                x.Description == curExpectedVault.Description &&
                !x.HasBeenUnlockedAtLeastOnce);
            }
            else
            {
                matchedVault = actualVaults.SingleOrDefault(x =>
                x.CloudProviderId == curExpectedVault.CloudProviderId &&
                x.CloudProviderPath == curExpectedVault.CloudProviderPath &&
                x.Name == curExpectedVault.Name &&
                x.Description == curExpectedVault.Description &&
                !x.HasBeenUnlockedAtLeastOnce);
            }

            return matchedVault != null;
        }

        return false;
    }
}
