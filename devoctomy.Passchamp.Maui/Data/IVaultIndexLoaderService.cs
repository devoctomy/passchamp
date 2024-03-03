using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Maui.Models;

namespace devoctomy.Passchamp.Maui.Data;

public interface IVaultIndexLoaderService
{
    public IReadOnlyList<VaultIndex> Vaults { get; }
    public Task LoadAsync(CancellationToken cancellationToken);
    public Task AddAsync(
        VaultIndex vaultIndex,
        CancellationToken cancellationToken);
    public Task AddFromCloudProviderAsync(
        CloudStorageProviderConfigRef cloudStorageProviderConfigRef,
        string cloudProviderPath,
        CancellationToken cancellationToken);
    public Task RemoveAsync(
        VaultIndex vaultIndex,
        CancellationToken cancellationToken);
}
