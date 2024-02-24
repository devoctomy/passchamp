using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Presets;
using devoctomy.Passchamp.Core.Services;
using devoctomy.Passchamp.Core.Vault;
using devoctomy.Passchamp.Maui.Models;
using System.Net;

namespace devoctomy.Passchamp.Maui.Data;

public class VaultCreatorService : IVaultCreatorService
{
    private readonly IIOService _ioService;
    private readonly IGraphFactory _graphFactory;
    private readonly IEnumerable<IGraphPresetSet> _graphPresetSets;

    public VaultCreatorService(
        IIOService ioService,
        IGraphFactory graphFactory,
        IEnumerable<IGraphPresetSet> graphPresetSets)
    {
        _ioService = ioService;
        _graphFactory = graphFactory;
        _graphPresetSets = graphPresetSets;
    }

    public async Task<Vault> Create(
        VaultIndex vaultIndex,
        IGraphPresetSet presetSet,
        CloudStorageProviderConfigRef cloudStorageProviderConfigRef,
        Func<Type, INode> InstantiateNode,
        CancellationToken cancellationToken)
    {
        var vault = new Vault
        {
            Name = vaultIndex.Name,
            Description = vaultIndex.Description,
        };

        using var outputStream = _ioService.OpenNewWrite("e:/temp/pop.vault"); //new MemoryStream();
        var parameters = new Dictionary<string, object>
            {
                { "SaltLength", 16 },
                { "IvLength", 16 },
                { "KeyLength", 32 },
                { "Passphrase", new NetworkCredential(null, "password123").SecurePassword },
                { "OutputStream", outputStream },
                { "Vault", vault },
            };
        var (encrypt, decrypt) = _graphFactory.LoadPresetSet(
            _graphPresetSets.Single(x => x.Id == vaultIndex.GraphPresetSetId),
            InstantiateNode,
            parameters);

        await encrypt.ExecuteAsync(cancellationToken);

        return vault;
    }
}
