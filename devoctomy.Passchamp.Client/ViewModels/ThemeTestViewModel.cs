using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Maui.Services;
using System.Windows.Input;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class ThemeTestViewModel : BaseAppShellPageViewModel
{
    private readonly IShellNavigationService _shellNavigationService;
    private readonly IThemeAwareImageResourceService _themeAwareImageResourceService;

    public ThemeTestViewModel(
        IShellNavigationService shellNavigationService,
        IThemeAwareImageResourceService themeAwareImageResourceService)
    {
        _shellNavigationService = shellNavigationService;
        _themeAwareImageResourceService = themeAwareImageResourceService;
        SetupMenuItems();
    }

    [RelayCommand]
    private void DarkTheme(object param)
    {
        Application.Current.UserAppTheme = AppTheme.Dark;
        Task.Run(RefreshMenuItems);
    }

    [RelayCommand]
    private void LightTheme(object param)
    {
        Application.Current.UserAppTheme = AppTheme.Light;
        Task.Run(RefreshMenuItems);
    }

    protected override Task OnFirstAppearanceAsync()
    {
        return Task.CompletedTask;
    }

    protected override void OnSetupMenuItems()
    {
        MenuItems.Add(new MenuItem
        {
            Text = "Light Theme",
            Command = LightThemeCommand,
            IconImageSource = _themeAwareImageResourceService.Get("bulb_02")
        });
        MenuItems.Add(new MenuItem
        {
            Text = "Dark Theme",
            Command = DarkThemeCommand,
            IconImageSource = _themeAwareImageResourceService.Get("bulb_03")
        });
        MenuItems.Add(new MenuItem
        {
            Text = "Accept",
            Command = AcceptCommand,
            IconImageSource = _themeAwareImageResourceService.Get("check")
        });
        MenuItems.Add(new MenuItem
        {
            Text = "Cancel",
            Command = CancelCommand,
            IconImageSource = _themeAwareImageResourceService.Get("close")
        });
    }

    [RelayCommand]
    private async Task Cancel(object param)
    {
        await _shellNavigationService.GoBackAsync();
    }

    [RelayCommand]
    private async Task Accept(object param)
    {
        await _shellNavigationService.GoToAsync("//Vaults");
    }
}
