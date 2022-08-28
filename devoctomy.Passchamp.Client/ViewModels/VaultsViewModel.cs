using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.Pages;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Vault;
using devoctomy.Passchamp.Maui.Data;
using devoctomy.Passchamp.Maui.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public partial class VaultsViewModel : BaseAppShellPageViewModel
    {
        public ICommand SettingsCommand { get; }
        public IAsyncRelayCommand AddVaultCommand { get; }
        public IAsyncRelayCommand EditSelectedVaultCommand { get; }
        public IAsyncRelayCommand RemoveSelectedVaultCommand { get; }

        [ObservableProperty]
        private ObservableCollection<VaultIndex> vaults;

        [ObservableProperty]
        private Vault selectedVaultIndex = null;

        private readonly ICloudStorageProviderConfigLoaderService _cloudStorageProviderConfigLoaderService;
        private readonly IVaultLoaderService _vaultLoaderService;
        private readonly static SemaphoreSlim _loaderLock = new(1, 1);
        private bool _loaded = false;

        public VaultsViewModel(
            ICloudStorageProviderConfigLoaderService cloudStorageProviderConfigLoaderService,
            IVaultLoaderService vaultLoaderService)
        {
            SettingsCommand = new Command(SettingsCommandHandler);
            AddVaultCommand = new AsyncRelayCommand(AddVaultCommandHandler);
            EditSelectedVaultCommand = new AsyncRelayCommand(EditSelectedVaultCommandHandler);
            RemoveSelectedVaultCommand = new AsyncRelayCommand(RemoveSelectedVaultCommandHandler);
            _cloudStorageProviderConfigLoaderService = cloudStorageProviderConfigLoaderService;
            _vaultLoaderService = vaultLoaderService;
            Vaults = new ObservableCollection<VaultIndex>();
            SetupMenuItems();
        }

        public override void OnSetupMenuItems()
        {
            MenuItems.Add(new MenuItem
            {
                Text = "Create Vault"
            });
            MenuItems.Add(new MenuItem
            {
                Text = "Add Existing Vault"
            });
            MenuItems.Add(new MenuItem
            {
                Text = "Synchronise"
            });
            MenuItems.Add(new MenuItem
            {
                Text = "Settings",
                Command = SettingsCommand
            });
        }

        public override async Task Return(BaseViewModel viewModel)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();

            if (viewModel == null)
            {
                return;
            }
        }

        public override async Task OnAppearingAsync()
        {
            await _loaderLock.WaitAsync();
            try
            {
                if (!_loaded)
                {
                    await _vaultLoaderService.LoadAsync(CancellationToken.None);
                    Vaults = new ObservableCollection<VaultIndex>(_vaultLoaderService.Vaults);
                    _loaded = true;
                }

                var appShellViewModel = Shell.Current.BindingContext as AppShellViewModel;
                await appShellViewModel.OnCurrentPageChangeAsync();
            }
            finally
            {
                _loaderLock.Release();
            }
        }

        private void SettingsCommandHandler(object param)
        {
            //var settingsPage = (SettingsPage)MauiProgram.MauiApp.Services.GetService(typeof(SettingsPage));
            //Shell.Current.Navigation.PushModalAsync(settingsPage);
            Shell.Current.GoToAsync("//Settings");
        }

        private async Task AddVaultCommandHandler()
        {
            var viewModel = new VaultEditorViewModel(this);
            var page = new Pages.VaultEditorPage(viewModel);
            await Application.Current.MainPage.Navigation.PushModalAsync(page, true);
        }

        private async Task EditSelectedVaultCommandHandler()
        {
            await Task.Yield();
            /*if (SelectedCloudStorageProviderConfigRef == null)
            {
                return;
            }

            var config = await _cloudStorageProviderConfigLoaderService.UnpackConfigAsync<AmazonS3CloudStorageProviderConfig>(
                SelectedCloudStorageProviderConfigRef.Id,
                CancellationToken.None);
            config = (AmazonS3CloudStorageProviderConfig)config.Clone();

            var viewModel = new CloudStorageProviderEditorViewModel(
                config,
                this);
            var page = new Pages.CloudStorageProviderEditorPage(viewModel);
            await Application.Current.MainPage.Navigation.PushModalAsync(page, true);*/
        }

        private async Task RemoveSelectedVaultCommandHandler()
        {
            await Task.Yield();
            /*if (SelectedCloudStorageProviderConfigRef == null)
            {
                return;
            }

            var selected = SelectedCloudStorageProviderConfigRef;
            CloudStorageProviderConfigRefs.Remove(selected);
            await _cloudStorageProviderConfigLoaderService.RemoveAsync(
                selected.Id,
                CancellationToken.None);
            SelectedCloudStorageProviderConfigRef = null;*/
        }
    }
}
