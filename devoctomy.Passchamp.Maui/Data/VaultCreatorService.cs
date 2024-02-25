using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Presets;
using devoctomy.Passchamp.Core.Services;
using devoctomy.Passchamp.Core.Vault;
using devoctomy.Passchamp.Maui.IO;
using devoctomy.Passchamp.Maui.Models;
using System.Net;

namespace devoctomy.Passchamp.Maui.Data;

public class VaultCreatorService : IVaultCreatorService
{
    private readonly IIOService _ioService;
    private readonly IGraphFactory _graphFactory;
    private readonly IEnumerable<IGraphPresetSet> _graphPresetSets;
    private readonly IVaultLoaderService _vaultLoaderService;
    private readonly IPathResolver _pathResolver;

    public VaultCreatorService(
        IIOService ioService,
        IGraphFactory graphFactory,
        IEnumerable<IGraphPresetSet> graphPresetSets,
        IVaultLoaderService vaultLoaderService,
        IPathResolver pathResolver)
    {
        _ioService = ioService;
        _graphFactory = graphFactory;
        _graphPresetSets = graphPresetSets;
        _vaultLoaderService = vaultLoaderService;
        _pathResolver = pathResolver;
    }

    public async Task<Vault> CreateAsync(
        VaultCreationOptions options,
        Func<Type, INode> InstantiateNode,
        CancellationToken cancellationToken)
    {
        var vault = new Vault
        {
            Name = options.Name,
            Description = options.Description,
        };

        // !!! This could be nicer
        var vaultDir = _pathResolver.Resolve($"{{{CommonPaths.ExternalCommonAppData}}}{{{CommonPaths.AppData}}}{{{CommonPaths.Vaults}}}");
        _ioService.CreatePathDirectory(vaultDir);
        var vaultPath = $"{vaultDir}{vault.Id}.vault";

        using var outputStream = _ioService.OpenNewWrite(vaultPath);
        var parameters = new Dictionary<string, object>
            {
                { "SaltLength", 16 },
                { "IvLength", 16 },
                { "KeyLength", 32 },
                { "Passphrase", options.Passphrase },
                { "OutputStream", outputStream },
                { "Vault", vault },
            };
        var (encrypt, decrypt) = _graphFactory.LoadPresetSet(
            _graphPresetSets.Single(x => x.Id == options.GraphPresetSetId),
            InstantiateNode,
            parameters);

        await encrypt.ExecuteAsync(cancellationToken);

        var index = new VaultIndex
        {
            Name = vault.Name,
            Description = vault.Description,
            GraphPresetSetId = options.GraphPresetSetId,
            CloudProviderId = options.CloudProviderId,
            CloudProviderPath = ReplacePathTokens(options.CloudProviderPath, vault),
        };
        await _vaultLoaderService.AddAsync(index, cancellationToken);

        return vault;
    }

    private static string ReplacePathTokens(
        string path,
        Vault vault)
    {
        var replaced = path;
        replaced = replaced.Replace("{id}", vault.Id);
        replaced = replaced.Replace("{name}", vault.Name);
        return replaced;
    }
}
