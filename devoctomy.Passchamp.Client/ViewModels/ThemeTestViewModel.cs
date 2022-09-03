using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Maui.Services;
using System.Windows.Input;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class ThemeTestViewModel : BaseAppShellPageViewModel
{
    public ICommand AcceptCommand { get; }
    public ICommand CancelCommand { get; }

    private readonly IShellNavigationService _shellNavigationService;
    private readonly IThemeAwareImageResourceService _themeAwareImageResourceService;

    public ThemeTestViewModel(
        IShellNavigationService shellNavigationService,
        IThemeAwareImageResourceService themeAwareImageResourceService)
    {
        AcceptCommand = new Command(AcceptCommandHandler);
        CancelCommand = new Command(CancelCommandHandler);
        _shellNavigationService = shellNavigationService;
        _themeAwareImageResourceService = themeAwareImageResourceService;
        SetupMenuItems();
    }

    public override Task OnFirstAppearanceAsync()
    {
        return Task.CompletedTask;
    }

    public override void OnSetupMenuItems()
    {
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
