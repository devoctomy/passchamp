using devoctomy.Passchamp.Core.Vault;

namespace devoctomy.Passchamp.Maui.Data;

public interface IVaultCreatorService
{
    public Task<Vault> CreateAsync(
        VaultCreationOptions options,
        Func<Type, Core.Graph.INode> InstantiateNode,
        CancellationToken cancellationToken);
}
