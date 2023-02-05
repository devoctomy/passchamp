using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.Pages;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Maui.Data;
using devoctomy.Passchamp.Maui.Models;
using devoctomy.Passchamp.Maui.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class VaultsViewModel : BaseAppShellPageViewModel
{
    public ICommand SettingsCommand { get; }
    public ICommand ThemeTestCommand { get; }
    public ICommand CreateVaultCommand { get; }
    public ICommand AddVaultCommand { get; }
    public ICommand EditSelectedVaultCommand { get; }
    public IAsyncRelayCommand RemoveSelectedVaultCommand { get; }

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
        SettingsCommand = new Command(SettingsCommandHandler);
        ThemeTestCommand = new Command(ThemeTestCommandHandler);
        AddVaultCommand = new Command(AddVaultCommandHandler);
        AddVaultCommand = new Command(CreateVaultCommandHandler);
        EditSelectedVaultCommand = new Command(EditSelectedVaultCommandHandler);
        RemoveSelectedVaultCommand = new AsyncRelayCommand(RemoveSelectedVaultCommandHandler);
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
            Command = AddVaultCommand,
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

        if (viewModel == null)
        {
            return;
        }
    }

    protected override async Task OnFirstAppearanceAsync()
    {
        await _vaultLoaderService.LoadAsync(CancellationToken.None);
        Vaults = new ObservableCollection<VaultIndex>(_vaultLoaderService.Vaults);
    }

    private void SettingsCommandHandler(object param)
    {
        _shellNavigationService.GoToAsync("//Settings");
    }

    private void ThemeTestCommandHandler(object param)
    {
        _shellNavigationService.GoToAsync("//ThemeTest");
    }

    private void AddVaultCommandHandler(object param)
    {
        var viewModel = new VaultEditorViewModel(this);
        var page = new VaultEditorPage(viewModel);
        Application.Current.MainPage.Navigation.PushModalAsync(page, true);
    }

    private void CreateVaultCommandHandler(object param)
    {
        var viewModel = new VaultEditorViewModel(this);
        var page = new Pages.VaultEditorPage(viewModel);
        Application.Current.MainPage.Navigation.PushModalAsync(page, true);
    }

    private void EditSelectedVaultCommandHandler()
    {
        if (SelectedVaultIndex == null)
        {
            return;
        }

        var vaultEditorPage = (VaultEditorPage)MauiProgram.MauiApp.Services.GetService(typeof(VaultEditorPage));
        Shell.Current.Navigation.PushModalAsync(vaultEditorPage);
    }

    private async Task RemoveSelectedVaultCommandHandler()
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
