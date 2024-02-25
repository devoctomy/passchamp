using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Maui.Exceptions;
using devoctomy.Passchamp.Maui.Models;
using Newtonsoft.Json;

namespace devoctomy.Passchamp.Maui.Data;

public class VaultLoaderService : IVaultLoaderService
{
    public IReadOnlyList<VaultIndex> Vaults => _vaults;

    private readonly VaultLoaderServiceOptions _options;
    private readonly IIOService _ioService;

    private List<VaultIndex> _vaults;

    [ActivatorUtilitiesConstructor]
    public VaultLoaderService(
        VaultLoaderServiceOptions options,
        IIOService ioService)
    {
        _options = options;
        _ioService = ioService;
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

    public async Task AddAsync(
        VaultIndex vaultIndex, 
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(vaultIndex.GraphPresetSetId))
        {
            throw new ArgumentException("Vault index missing GraphPresetSetId");
        }

        if (string.IsNullOrEmpty(vaultIndex.CloudProviderId))
        {
            throw new ArgumentException("Vault index missing CloudProviderId");
        }

        if (string.IsNullOrEmpty(vaultIndex.CloudProviderPath))
        {
            throw new ArgumentException("Vault index missing CloudProviderPath");
        }

        if(Vaults.Any(x => x.Id == vaultIndex.Id))
        {
            throw new VaultAlreadyIndexedException(vaultIndex.Id);
        }

        _vaults.Add(vaultIndex);
        await SaveAsync(cancellationToken);
    }
}
