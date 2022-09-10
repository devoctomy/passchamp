using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Maui.Services;
using System.Windows.Input;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class ThemeTestViewModel : BaseAppShellPageViewModel
{
    public ICommand LightThemeCommand { get; }
    public ICommand DarkThemeCommand { get; }
    public ICommand AcceptCommand { get; }
    public ICommand CancelCommand { get; }

    private readonly IShellNavigationService _shellNavigationService;
    private readonly IThemeAwareImageResourceService _themeAwareImageResourceService;

    public ThemeTestViewModel(
        IShellNavigationService shellNavigationService,
        IThemeAwareImageResourceService themeAwareImageResourceService)
    {
        LightThemeCommand = new Command(LightThemeCommandHandler);
        DarkThemeCommand = new Command(DarkThemeCommandHandler);
        AcceptCommand = new Command(AcceptCommandHandler);
        CancelCommand = new Command(CancelCommandHandler);
        _shellNavigationService = shellNavigationService;
        _themeAwareImageResourceService = themeAwareImageResourceService;
        SetupMenuItems();
    }

    private void DarkThemeCommandHandler(object param)
    {
        Application.Current.UserAppTheme = AppTheme.Dark;
        Task.Run(RefreshMenuItems);
    }

    private void LightThemeCommandHandler(object param)
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

    private void CancelCommandHandler(object param)
    {
        _shellNavigationService.GoBackAsync();
    }

    private void AcceptCommandHandler(object param)
    {
        _shellNavigationService.GoToAsync("//Vaults");
    }
}
