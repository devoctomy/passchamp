using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.Config;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Config;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Maui.Services;
using System.Windows.Input;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class SettingsViewModel : BaseAppShellPageViewModel
{
    [ObservableProperty]
    private GeneralSettingsViewModel generalSettings;

    [ObservableProperty]
    private CloudSettingsViewModel cloudSettings;

    private readonly IShellNavigationService _shellNavigationService;
    private readonly IThemeAwareImageResourceService _themeAwareImageResourceService;
    private readonly IApplicationConfigLoaderService<AppConfig> _applicationConfigLoaderService;

    public SettingsViewModel(
        GeneralSettingsViewModel generalSettingsViewModel,
        CloudSettingsViewModel cloudSettingsViewModel,
        IShellNavigationService shellNavigationService,
        IThemeAwareImageResourceService themeAwareImageResourceService,
        IApplicationConfigLoaderService<AppConfig> applicationConfigLoaderService)
    {
        GeneralSettings = generalSettingsViewModel;
        CloudSettings = cloudSettingsViewModel;
        _shellNavigationService = shellNavigationService;
        _themeAwareImageResourceService = themeAwareImageResourceService;
        _applicationConfigLoaderService = applicationConfigLoaderService;
        SetupMenuItems();
    }

    protected override async Task OnFirstAppearanceAsync()
    {
        await CloudSettings.Init();
        await _applicationConfigLoaderService.LoadAsync(CancellationToken.None);
        GeneralSettings.Theme = _applicationConfigLoaderService.Config.Theme;
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

    [RelayCommand]
    private async Task Cancel(object param)
    {
        await _shellNavigationService.GoBackAsync();
    }

    [RelayCommand]
    private async Task Accept(object param)
    {
        _applicationConfigLoaderService.Config.Theme = GeneralSettings.Theme;
        await _applicationConfigLoaderService.SaveAsync(CancellationToken.None);
        await _shellNavigationService.GoToAsync("//Vaults");
    }

    [RelayCommand]
    private void TabChanged(object param)
    {
        // Handle tab changed event here
    }
}
