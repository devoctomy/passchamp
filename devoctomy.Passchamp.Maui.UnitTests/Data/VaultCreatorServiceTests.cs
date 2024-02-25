using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Presets;
using devoctomy.Passchamp.Core.Services;
using devoctomy.Passchamp.Maui.Data;
using devoctomy.Passchamp.Maui.IO;
using devoctomy.Passchamp.Maui.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Maui.UnitTests.Data;

public class VaultCreatorServiceTests
{
    [Fact]
    public async Task GivenOptions_AndInstantiateNodeFunc_WhenCreateAsync_ThenVaultCreatedCorrectly_AndVaultAddedToVaultLoaderService()
    {
        // Arrange
        var mockIoService = new Mock<IIOService>();
        var mockGraphFactory = new Mock<IGraphFactory>();
        var mockGraphEncrypt = new Mock<IGraph>();
        var mockGraphDecrypt = new Mock<IGraph>();
        var mockGraphPresetSet = new Mock<IGraphPresetSet>();
        var graphPresetSets = new List<IGraphPresetSet>
        {
            mockGraphPresetSet.Object
        };
        var mockVaultLoaderService = new Mock<IVaultLoaderService>();
        var mockPathResolverService = new Mock<IPathResolverService>();
        var sut = new VaultCreatorService(
            mockIoService.Object,
            mockGraphFactory.Object,
            graphPresetSets,
            mockVaultLoaderService.Object,
            mockPathResolverService.Object);
        using var outputStream = new MemoryStream();
        var passphrase = new SecureString();
        var outputDir = "c:/pop/pip/";
        var graphPresetSetId = "Foobar";

        var options = new VaultCreationOptions
        {
            Name = "Test",
            CloudProviderPath = "folder/{id}.{name}.vault",
            Passphrase = passphrase,
            GraphPresetSetId = graphPresetSetId
        };

        mockGraphFactory.Setup(x => x.LoadPresetSet(
            It.IsAny<IGraphPresetSet>(),
            It.IsAny<Func<Type, INode>>(),
            It.IsAny<Dictionary<string, object>>()))
            .Returns((mockGraphEncrypt.Object, mockGraphDecrypt.Object));

        mockGraphPresetSet.SetupGet(x => x.Id)
            .Returns(graphPresetSetId);

        mockPathResolverService.Setup(x => x.Resolve(
            It.IsAny<string>()))
            .Returns(outputDir);

        mockIoService.Setup(x => x.OpenNewWrite(
            It.IsAny<string>()))
            .Returns(outputStream);

        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // Act
        await sut.CreateAsync(
            options,
            InstantiateNode,
            cancellationToken);

        // Assert
        mockPathResolverService.Verify(x => x.Resolve(
            It.IsAny<string>()), Times.Once);
        mockIoService.Verify(x => x.CreatePathDirectory(
            It.Is<string>(y => y == outputDir)), Times.Once);
        mockIoService.Verify(x => x.OpenNewWrite(
            It.Is<string>(y => y.StartsWith(outputDir) && y.EndsWith(".vault"))), Times.Once);
        mockGraphFactory.Verify(x => x.LoadPresetSet(
            It.Is<IGraphPresetSet>(y => y == mockGraphPresetSet.Object),
            It.IsAny<Func<Type, INode>>(),
            It.Is<Dictionary<string, object>>(y => y["Passphrase"] == passphrase &&  y["OutputStream"] == outputStream)), Times.Once);
        mockGraphEncrypt.Verify(x => x.ExecuteAsync(
            It.Is<CancellationToken>(y => y == cancellationToken)), Times.Once);
        mockGraphDecrypt.Verify(x => x.ExecuteAsync(
            It.Is<CancellationToken>(y => y == cancellationToken)), Times.Never);
        mockVaultLoaderService.Verify(x => x.AddAsync(
            It.Is<VaultIndex>(y => y.Name == options.Name && y.Description == options.Description),
            It.Is<CancellationToken>(y => y == cancellationToken)), Times.Once);
    }

    private INode InstantiateNode(Type type)
    {
        throw new NotImplementedException();
    }
}
