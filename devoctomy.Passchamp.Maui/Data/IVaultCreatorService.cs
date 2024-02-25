using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Graph.Presets;
using devoctomy.Passchamp.Core.Vault;
using devoctomy.Passchamp.Maui.Models;

namespace devoctomy.Passchamp.Maui.Data;

public interface IVaultCreatorService
{
    public Task<Vault> CreateAsync(
        VaultCreationOptions options,
        Func<Type, Core.Graph.INode> InstantiateNode,
        CancellationToken cancellationToken);
}
