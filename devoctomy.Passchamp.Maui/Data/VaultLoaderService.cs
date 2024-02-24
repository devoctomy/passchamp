using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Services;
using devoctomy.Passchamp.Core.Vault;
using devoctomy.Passchamp.Maui.Exceptions;
using devoctomy.Passchamp.Maui.Models;
using Newtonsoft.Json;
using System.Net;

namespace devoctomy.Passchamp.Maui.Data;

public class VaultLoaderService : IVaultLoaderService
{
    public IReadOnlyList<VaultIndex> Vaults => _vaults;

    private readonly VaultLoaderServiceOptions _options;
    private readonly IIOService _ioService;
    private readonly IGraphFactory _graphFactory;

    private List<VaultIndex> _vaults;

    [ActivatorUtilitiesConstructor]
    public VaultLoaderService(
        VaultLoaderServiceOptions options,
        IIOService ioService,
        IGraphFactory graphFactory)
    {
        _options = options;
        _ioService = ioService;
        _graphFactory = graphFactory;
        _vaults = [];
    }

    // Used purely for unit testing purposes
    public VaultLoaderService(
        VaultLoaderServiceOptions options,
        IIOService ioService,
        List<VaultIndex> vaults)
    {
        _options = options;
        _ioService = ioService;
        _vaults = vaults;
    }

    public async Task LoadAsync(CancellationToken cancellationToken)
    {
        var fullPath = $"{_options.Path}{_options.FileName}";
        _ioService.CreatePathDirectory(fullPath);
        if (_ioService.Exists(fullPath))
        {
            var jsonRaw = await _ioService.ReadAllTextAsync(
                fullPath,
                cancellationToken);
            _vaults = JsonConvert.DeserializeObject<List<VaultIndex>>(jsonRaw);
        }
    }

    public async Task AddFromCloudProviderAsync(
        CloudStorageProviderConfigRef cloudStorageProviderConfigRef,
        string cloudProviderPath,
        CancellationToken cancellationToken)
    {
        var index = new VaultIndex
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Not yet decrypted",
            Description = "Not yet decrypted",
            CloudProviderId = cloudStorageProviderConfigRef.ProviderServiceTypeId,
            CloudProviderPath = cloudProviderPath,
        };
        _vaults.Add(index);
        await SaveAsync(cancellationToken);
    }

    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        var fullPath = $"{_options.Path}{_options.FileName}";
        _ioService.CreatePathDirectory(fullPath);
        var jsonRaw = JsonConvert.SerializeObject(Vaults);
        await _ioService.WriteDataAsync(
            fullPath,
            jsonRaw,
            cancellationToken);
    }

    public async Task RemoveAsync(
        VaultIndex vaultIndex,
        CancellationToken cancellationToken)
    {
        if(!_vaults.Contains(vaultIndex))
        {
            throw new VaultIndexNotFoundException(vaultIndex.Id);
        }

        _vaults.Remove(vaultIndex);
        await SaveAsync(cancellationToken);
    }

    public Task Create(
        VaultIndex vaultIndex,
        CloudStorageProviderConfigRef cloudStorageProviderConfigRef,
        Func<Type, INode> InstantiateNode,
        CancellationToken cancellationToken)
    {
        var vault = new Vault
        {
            Name = vaultIndex.Name,
            Description = vaultIndex.Description,
        };

        var parameters = new Dictionary<string, object>
            {
                { "SaltLength", 16 },
                { "IvLength", 16 },
                { "KeyLength", 32 },
                { "Passphrase", new NetworkCredential(null, "password123").SecurePassword },
                { "InputStream", null },
                { "OutputStream", null },
                { "PlainText", "Hello World!" }, // !!! This needs to be a vault which gets serialised to JSON during encryption phase
            };
        var presetSet = _graphFactory.LoadPresetSet(
            null,
            InstantiateNode,
            parameters);

        presetSet.encrypt.ExecuteAsync(cancellationToken);

        return null;
    }
}
