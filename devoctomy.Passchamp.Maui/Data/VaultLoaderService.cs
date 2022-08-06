using devoctomy.Passchamp.Client.Models;
using devoctomy.Passchamp.Core.Data;
using Newtonsoft.Json;

namespace devoctomy.Passchamp.Maui.Data
{
    public class VaultLoaderService : IVaultLoaderService
    {
        public IReadOnlyList<VaultIndex> Vaults => _vaults;

        private readonly VaultLoaderServiceOptions _options;
        private readonly IIOService _ioService;

        private List<VaultIndex> _vaults;

        public VaultLoaderService(
            VaultLoaderServiceOptions options,
            IIOService ioService)
        {
            _options = options;
            _ioService = ioService;
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
    }
}
