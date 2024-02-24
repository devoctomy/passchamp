using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Vault;
using devoctomy.Passchamp.Maui.Models;

namespace devoctomy.Passchamp.Maui.Data;

public interface IVaultLoaderService
{
    public IReadOnlyList<VaultIndex> Vaults { get; }
    public Task LoadAsync(CancellationToken cancellationToken);
    public Task Create(
        VaultIndex vaultIndex,
        CloudStorageProviderConfigRef cloudStorageProviderConfigRef,
        Func<Type, INode> InstantiateNode,
        CancellationToken cancellationToken);
    public Task AddFromCloudProviderAsync(
        CloudStorageProviderConfigRef cloudStorageProviderConfigRef,
        string cloudProviderPath,
        CancellationToken cancellationToken);
    public Task RemoveAsync(
        VaultIndex vaultIndex,
        CancellationToken cancellationToken);
}
