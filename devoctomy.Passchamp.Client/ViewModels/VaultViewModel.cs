using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.Pages;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Maui.Services;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class VaultViewModel : BaseAppShellPageViewModel
{
    private readonly IShellNavigationService _shellNavigationService;
    private readonly IThemeAwareImageResourceService _themeAwareImageResourceService;

    public VaultViewModel(
        IShellNavigationService shellNavigationService,
        IThemeAwareImageResourceService themeAwareImageResourceService)
    {
        _shellNavigationService = shellNavigationService;
        _themeAwareImageResourceService = themeAwareImageResourceService;
        SetupMenuItems();
    }

    protected override void OnSetupMenuItems()
    {
        MenuItems.Add(new MenuItem
        {
            Text = "Lock Vault",
            Command = LockVaultCommand,
            IconImageSource = _themeAwareImageResourceService.Get("lock_wf")
        });

        MenuItems.Add(new MenuItem
        {
            Text = "Create Credential",
            Command = CreateCredentialCommand,
            IconImageSource = null
        });

        MenuItems.Add(new MenuItem
        {
            Text = "Import From CSV",
            Command = ImportFromCSVCommand,
            IconImageSource = null
        });

        MenuItems.Add(new MenuItem
        {
            Text = "Export To CSV",
            Command = ImportFromCSVCommand,
            IconImageSource = null
        });

        MenuItems.Add(new MenuItem
        {
            Text = "Report",
            Command = ReportCommand,
            IconImageSource = null
        });

        MenuItems.Add(new MenuItem
        {
            Text = "Audit",
            Command = AuditCommand,
            IconImageSource = null
        });
    }

    protected override async Task OnFirstAppearanceAsync()
    {
        await Task.Yield();
    }

    public override async Task Return(BaseViewModel viewModel)
    {
        await Application.Current.MainPage.Navigation.PopModalAsync(); // Handle page callbacks here
    }

    [RelayCommand]
    private async Task LockVault(object param)
    {
        await _shellNavigationService.GoToAsync("//Vaults");
    }

    [RelayCommand]
    private async Task CreateCredential(object param)
    {
        // !!! Can we do this better?
        var viewModel = new CredentialEditorViewModel(this);
        var page = new CredentialEditorPage(viewModel);
        await Application.Current.MainPage.Navigation.PushModalAsync(page, true);
    }

    [RelayCommand]
    private async Task ImportFromCSV(object param)
    {
        // show import page
        await Task.Yield();
    }

    [RelayCommand]
    private async Task ExportToSV(object param)
    {
        // show import page
        await Task.Yield();
    }

    [RelayCommand]
    private async Task Report(object param)
    {
        // show report page
        await Task.Yield();
    }

    [RelayCommand]
    private async Task Audit(object param)
    {
        // show audit page
        await Task.Yield();
    }
}
