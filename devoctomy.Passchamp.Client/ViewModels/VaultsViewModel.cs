using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.Pages;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Maui.Data;
using devoctomy.Passchamp.Maui.Models;
using devoctomy.Passchamp.Maui.Services;
using System.Collections.ObjectModel;

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

    public VaultsViewModel(
        IVaultLoaderService vaultLoaderService,
        IShellNavigationService shellNavigationService,
        IThemeAwareImageResourceService themeAwareImageResourceService)
    {
        _vaultLoaderService = vaultLoaderService;
        _shellNavigationService = shellNavigationService;
        _themeAwareImageResourceService = themeAwareImageResourceService;
        Vaults = new ObservableCollection<VaultIndex>();
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
        // !!! We shouldn't display this tab in final builds, it's only for testing at current
        MenuItems.Add(new MenuItem
        {
            Text = "Theme Test",
            Command = ThemeTestCommand,
            IconImageSource = _themeAwareImageResourceService.Get("polaroid_01_wf")
        });
    }

    public override async Task Return(BaseViewModel viewModel)
    {
        await Application.Current.MainPage.Navigation.PopModalAsync();
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
