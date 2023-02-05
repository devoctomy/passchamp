using CommunityToolkit.Mvvm.ComponentModel;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Maui.Services;
using System.Windows.Input;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class SettingsViewModel : BaseAppShellPageViewModel
{
    [ObservableProperty]
    private GeneralSettingsViewModel generalSettings;

    [ObservableProperty]
    private CloudSettingsViewModel cloudSettings;

    public ICommand AcceptCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand TabChangedCommand { get; }

    private readonly IShellNavigationService _shellNavigationService;
    private readonly IThemeAwareImageResourceService _themeAwareImageResourceService;

    public SettingsViewModel(
        GeneralSettingsViewModel generalSettingsViewModel,
        CloudSettingsViewModel cloudSettingsViewModel,
        IShellNavigationService shellNavigationService,
        IThemeAwareImageResourceService themeAwareImageResourceService)
    {
        GeneralSettings = generalSettingsViewModel;
        CloudSettings = cloudSettingsViewModel;

        AcceptCommand = new Command(AcceptCommandHandler);
        CancelCommand = new Command(CancelCommandHandler);
        TabChangedCommand = new Command(TabChangedCommandHandler);
        _shellNavigationService = shellNavigationService;
        _themeAwareImageResourceService = themeAwareImageResourceService;
        SetupMenuItems();
    }

    protected override async Task OnFirstAppearanceAsync()
    {
        await cloudSettings.Init();
    }

    protected override void OnSetupMenuItems()
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

    private void TabChangedCommandHandler(object param)
    {
    }
}
