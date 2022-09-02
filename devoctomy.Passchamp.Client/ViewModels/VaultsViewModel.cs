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
    public ICommand CreateVaultCommand { get; }
    public ICommand AddVaultCommand { get; }
    public ICommand EditSelectedVaultCommand { get; }
    public IAsyncRelayCommand RemoveSelectedVaultCommand { get; }

    [ObservableProperty]
    private ObservableCollection<VaultIndex> vaults;

    [ObservableProperty]
    private VaultIndex selectedVaultIndex = null;

    private readonly IVaultLoaderService _vaultLoaderService;
    private readonly IShellNavigationService _shellNavigationService;
    private readonly static SemaphoreSlim _loaderLock = new(1, 1);
    private bool _loaded = false;

    public VaultsViewModel(
        IVaultLoaderService vaultLoaderService,
        IShellNavigationService shellNavigationService)
    {
        SettingsCommand = new Command(SettingsCommandHandler);
        AddVaultCommand = new Command(AddVaultCommandHandler);
        AddVaultCommand = new Command(CreateVaultCommandHandler);
        EditSelectedVaultCommand = new Command(EditSelectedVaultCommandHandler);
        RemoveSelectedVaultCommand = new AsyncRelayCommand(RemoveSelectedVaultCommandHandler);
        _vaultLoaderService = vaultLoaderService;
        _shellNavigationService = shellNavigationService;
        Vaults = new ObservableCollection<VaultIndex>();
        SetupMenuItems();
    }

    public override void OnSetupMenuItems()
    {
        MenuItems.Add(new MenuItem
        {
            Text = "Create Vault",
            Command = AddVaultCommand,
            IconImageSource = "new_dark.png"
        });
        MenuItems.Add(new MenuItem
        {
            Text = "Add Existing Vault",
            Command = AddVaultCommand,
            IconImageSource = "add_dark.png"
        });
        MenuItems.Add(new MenuItem
        {
            Text = "Synchronise",
            IconImageSource = "synchronize_dark.png"
        });
        MenuItems.Add(new MenuItem
        {
            Text = "Settings",
            Command = SettingsCommand,
            IconImageSource = "settings_dark.png"
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
        _shellNavigationService.GoToAsync("//Settings");
    }

    private void AddVaultCommandHandler(object param)
    {
        var viewModel = new VaultEditorViewModel(this);
        var page = new Pages.VaultEditorPage(viewModel);
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
