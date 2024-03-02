using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Presets;
using devoctomy.Passchamp.Core.Services;
using devoctomy.Passchamp.Core.Vault;
using devoctomy.Passchamp.Maui.IO;

namespace devoctomy.Passchamp.Maui.Data;

public class VaultLoaderService : IVaultLoaderService
{
    private readonly IIOService _ioService;
    private readonly IGraphFactory _graphFactory;
    private readonly IEnumerable<IGraphPresetSet> _graphPresetSets;
    private readonly IPathResolverService _pathResolverService;

    public VaultLoaderService(
        IIOService ioService,
        IGraphFactory graphFactory,
        IEnumerable<IGraphPresetSet> graphPresetSets,
        IPathResolverService pathResolverService)
    {
        _ioService = ioService;
        _graphFactory = graphFactory;
        _graphPresetSets = graphPresetSets;
        _pathResolverService = pathResolverService;
    }

    public async Task<Vault> LoadAsync(
        VaultLoaderServiceOptions options,
        Func<Type, INode> InstantiateNode,
        CancellationToken cancellationToken)
    {
        var vaultDir = _pathResolverService.Resolve($"{{{CommonPaths.ExternalCommonAppData}}}{{{CommonPaths.Vaults}}}");
        var vaultPath = $"{vaultDir}{options.VaultIndex.Id}.vault";
        var inputStream = _ioService.OpenRead(vaultPath);

        var parameters = new Dictionary<string, object>
            {
                { "Passphrase", options.MasterPassphrase },
                { "InputStream", inputStream }
            };
        var (encrypt, decrypt) = _graphFactory.LoadPresetSet(
            _graphPresetSets.Single(x => x.Id == options.VaultIndex.GraphPresetSetId),
            InstantiateNode,
            parameters);

        await decrypt.ExecuteAsync(cancellationToken);

        return (Vault)decrypt.OutputPins["Vault"].ObjectValue;
    }
}
