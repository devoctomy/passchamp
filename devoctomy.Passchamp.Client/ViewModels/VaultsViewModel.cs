using CommunityToolkit.Mvvm.ComponentModel;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Vault;
using devoctomy.Passchamp.Maui.Data;
using devoctomy.Passchamp.Maui.Models;
using System.Collections.ObjectModel;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public partial class VaultsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<VaultIndex> vaults;

        [ObservableProperty]
        private Vault itemSelected = null;

        private readonly ICloudStorageProviderConfigLoaderService _cloudStorageProviderConfigLoaderService;
        private readonly IVaultLoaderService _vaultLoaderService;
        private readonly static SemaphoreSlim _loaderLock = new(1, 1);
        private bool _loaded = false;

        public VaultsViewModel(
            ICloudStorageProviderConfigLoaderService cloudStorageProviderConfigLoaderService,
            IVaultLoaderService vaultLoaderService)
        {
            _cloudStorageProviderConfigLoaderService = cloudStorageProviderConfigLoaderService;
            _vaultLoaderService = vaultLoaderService;
        }

        public override async Task OnAppearingAsync()
        {
            await _loaderLock.WaitAsync();
            try
            {
                if (!_loaded)
                {
                    await _vaultLoaderService.LoadAsync(CancellationToken.None);
                    vaults = new ObservableCollection<VaultIndex>(_vaultLoaderService.Vaults);

                    vaults.Add(new VaultIndex
                    {
                        Name = "Test",
                        Description = "Hello World!"
                    });
                    _loaded = true;
                }
            }
            finally
            {
                _loaderLock.Release();
            }
        }
    }
}
