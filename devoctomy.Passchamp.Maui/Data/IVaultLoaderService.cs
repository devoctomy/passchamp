using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Vault;

namespace devoctomy.Passchamp.Maui.Data;

public interface IVaultLoaderService
{
    public Task<Vault> LoadAsync(
        VaultLoaderServiceOptions options,
        Func<Type, INode> InstantiateNode,
        CancellationToken cancellationToken);
}
