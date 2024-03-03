using CommunityToolkit.Mvvm.Input;
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
    }

    protected override async Task OnFirstAppearanceAsync()
    {
        await Task.Yield();
    }

    [RelayCommand]
    private async Task LockVault(object param)
    {
        await _shellNavigationService.GoToAsync("//Vaults");
    }
}
