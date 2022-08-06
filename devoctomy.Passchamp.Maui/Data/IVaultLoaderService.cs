using devoctomy.Passchamp.Client.Models;

namespace devoctomy.Passchamp.Maui.Data
{
    public interface IVaultLoaderService
    {
        public IReadOnlyList<VaultIndex> Vaults { get; }
        public Task LoadAsync(CancellationToken cancellationToken);
    }
}
