﻿using devoctomy.Passchamp.Client.Models;
using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Vault;
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
            _vaults = new List<VaultIndex>();
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
    }
}
