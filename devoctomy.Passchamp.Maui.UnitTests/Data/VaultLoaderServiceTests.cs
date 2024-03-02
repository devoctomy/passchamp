using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Presets;
using devoctomy.Passchamp.Core.Services;
using devoctomy.Passchamp.Maui.Data;
using devoctomy.Passchamp.Maui.IO;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;

namespace devoctomy.Passchamp.Maui.UnitTests.Data;

public class VaultLoaderServiceTests
{
    [Fact]
    public async void GivenOptions_AndInstantiateNodeFunc_WhenLoadAsync_ThenLoadAndReturnVault()
    {
        // Arrange
        var mockIoService = new Mock<IIOService>();
        var mockGraphFactory = new Mock<IGraphFactory>();
        var mockGraphPresetSet = new Mock<IGraphPresetSet>();
        var mockDecryptGraph = new Mock<IGraph>();
        var graphPresetSets = new List<IGraphPresetSet>
        {
            mockGraphPresetSet.Object
        };
        var mockPathResolver = new Mock<IPathResolverService>();
        var mockOutputVaultPin = new Mock<IPin>();
        var sut = new VaultLoaderService(
            mockIoService.Object,
            mockGraphFactory.Object,
            graphPresetSets,
            mockPathResolver.Object);

        var cancellationTokenSource = new CancellationTokenSource();

        var graphPresetSetId = "123";
        mockGraphPresetSet.SetupGet(x => x.Id).Returns(graphPresetSetId);

        var options = new VaultLoaderServiceOptions
        {
            VaultIndex = new Models.VaultIndex
            {
                Id = "Foobar",
                GraphPresetSetId = graphPresetSetId
            },
            MasterPassphrase = new System.Security.SecureString()
        };

        var path = "c:/pop/pip/";

        mockPathResolver.Setup(x => x.Resolve(
            It.IsAny<string>()))
            .Returns(path);

        mockGraphFactory.Setup(x => x.LoadPresetSet(
            It.IsAny<IGraphPresetSet>(),
            InstantiateNode,
            It.IsAny<Dictionary<string, object>>())).Returns((null, mockDecryptGraph.Object));

        var vault = new Core.Vault.Vault();
        mockOutputVaultPin.SetupGet(x => x.ObjectValue).Returns(vault);

        var outputPins = new Dictionary<string, IPin>
        {
            { "Vault", mockOutputVaultPin.Object }
        };
        mockDecryptGraph.SetupGet(x => x.OutputPins).Returns(outputPins);

        // Act
        var result = await sut.LoadAsync(
            options,
            InstantiateNode,
            cancellationTokenSource.Token);

        // Assert
        mockPathResolver.Verify(x => x.Resolve(
            It.Is<string>(y => y == $"{{{CommonPaths.ExternalCommonAppData}}}{{{CommonPaths.Vaults}}}")), Times.Once);
        mockIoService.Verify(x => x.OpenRead(
            It.Is<string>(y => y == $"{path}{options.VaultIndex.Id}.vault")), Times.Once);
        mockGraphFactory.Verify(x => x.LoadPresetSet(
            It.Is<IGraphPresetSet>(y => y == mockGraphPresetSet.Object),
            InstantiateNode,
            It.IsAny<Dictionary<string, object>>()), Times.Once);
        mockDecryptGraph.Verify(x => x.ExecuteAsync(
            It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        Assert.Equal(vault, result);
    }

    private INode InstantiateNode(Type type)
    {
        return null;
    }
}
