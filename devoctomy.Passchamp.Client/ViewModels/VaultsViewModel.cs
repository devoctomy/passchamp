using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.Pages;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Services;
using devoctomy.Passchamp.Core.Vault;
using devoctomy.Passchamp.Maui.Data;
using devoctomy.Passchamp.Maui.Models;
using devoctomy.Passchamp.Maui.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class VaultsViewModel : BaseAppShellPageViewModel
{
    [ObservableProperty]
    private ObservableCollection<VaultIndex> vaults;

    [ObservableProperty]
    private VaultIndex selectedVaultIndex;

    private readonly IVaultLoaderService _vaultLoaderService;
    private readonly IShellNavigationService _shellNavigationService;
    private readonly IThemeAwareImageResourceService _themeAwareImageResourceService;
    private readonly IVaultCreatorService _vaultCreatorService;

    public VaultsViewModel(
        IVaultLoaderService vaultLoaderService,
        IShellNavigationService shellNavigationService,
        IThemeAwareImageResourceService themeAwareImageResourceService,
        IVaultCreatorService vaultCreatorService)
    {
        _vaultLoaderService = vaultLoaderService;
        _shellNavigationService = shellNavigationService;
        _themeAwareImageResourceService = themeAwareImageResourceService;
        _vaultCreatorService = vaultCreatorService;
        Vaults = vaultLoaderService.Vaults.ToObservableCollection();
        SetupMenuItems();
    }

    protected override void OnSetupMenuItems()
    {
        MenuItems.Add(new MenuItem
        {
            Text = "Create Vault",
            Command = CreateVaultCommand,
            IconImageSource = _themeAwareImageResourceService.Get("new")
        });
        MenuItems.Add(new MenuItem
        {
            Text = "Add Existing Vault",
            Command = AddVaultCommand,
            IconImageSource = _themeAwareImageResourceService.Get("add")
        });
        MenuItems.Add(new MenuItem
        {
            Text = "Synchronise",
            IconImageSource = _themeAwareImageResourceService.Get("synchronize")
        });
        MenuItems.Add(new MenuItem
        {
            Text = "Settings",
            Command = SettingsCommand,
            IconImageSource = _themeAwareImageResourceService.Get("settings")
        });
#if DEBUG
        MenuItems.Add(new MenuItem
        {
            Text = "Theme Test",
            Command = ThemeTestCommand,
            IconImageSource = _themeAwareImageResourceService.Get("polaroid_01_wf")
        });
#endif
    }

    public override async Task Return(BaseViewModel viewModel)
    {
        await Application.Current.MainPage.Navigation.PopModalAsync();
        if (viewModel is VaultEditorViewModel)
        {
            var vaultEditorViewModel = viewModel as VaultEditorViewModel;
            await _vaultCreatorService.CreateAsync(
                new VaultCreationOptions
                {
                    Name = vaultEditorViewModel.InfoViewModel.Name,
                    Description = vaultEditorViewModel.InfoViewModel.Description,
                    GraphPresetSetId = vaultEditorViewModel.SecurityViewModel.SelectedGraphPresetSet.Id,
                    CloudProviderId = vaultEditorViewModel.SyncViewModel.SelectedCloudStorageProviderConfigRef.Id,
                    CloudProviderPath = "/{id}.vault",
                    Passphrase = new NetworkCredential(null, vaultEditorViewModel.SecurityViewModel.MasterPassphrase).SecurePassword
                },
                InstantiateNode,
                CancellationToken.None);
        }
    }

    private INode InstantiateNode(Type type)
    {
        var node = (INode)MauiProgram.MauiApp.Services.GetService(type);
        return node;
    }

    protected override async Task OnFirstAppearanceAsync()
    {
        await _vaultLoaderService.LoadAsync(CancellationToken.None);
        Vaults = new ObservableCollection<VaultIndex>(_vaultLoaderService.Vaults);
    }

    [RelayCommand]
    private async Task Settings(object param)
    {
        await _shellNavigationService.GoToAsync("//Settings");
    }

    [RelayCommand]
    private async Task ThemeTest(object param)
    {
        await _shellNavigationService.GoToAsync("//ThemeTest");
    }

    [RelayCommand]
    private async Task AddVault(object param)
    {
        // !!! Can we do this better?
        var cloudStorageProviderConfigLoaderService = MauiProgram.MauiApp.Services.GetService<ICloudStorageProviderConfigLoaderService>();
        var viewModel = new VaultEditorViewModel(this, cloudStorageProviderConfigLoaderService);
        var page = new VaultEditorPage(viewModel);
        await Application.Current.MainPage.Navigation.PushModalAsync(page, true);
    }

    [RelayCommand]
    private async Task CreateVault(object param)
    {
        // Can we do this better?
        var cloudStorageProviderConfigLoaderService = MauiProgram.MauiApp.Services.GetService<ICloudStorageProviderConfigLoaderService>();
        var viewModel = new VaultEditorViewModel(this, cloudStorageProviderConfigLoaderService);
        await viewModel.Init();
        var page = new Pages.VaultEditorPage(viewModel);
        await Application.Current.MainPage.Navigation.PushModalAsync(page, true);
    }

    [RelayCommand]
    private async Task EditSelected()
    {
        if (SelectedVaultIndex == null)
        {
            return;
        }

        var vaultEditorPage = (VaultEditorPage)MauiProgram.MauiApp.Services.GetService(typeof(VaultEditorPage));
        await Shell.Current.Navigation.PushModalAsync(vaultEditorPage);
    }

    [RelayCommand]
    private async Task RemoveSelected()
    {
        if (SelectedVaultIndex == null)
        {
            return;
        }

        await _vaultLoaderService.RemoveAsync(
            SelectedVaultIndex,
            CancellationToken.None);
    }
}
