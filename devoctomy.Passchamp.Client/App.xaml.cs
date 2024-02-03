using devoctomy.Passchamp.Client.Config;
using devoctomy.Passchamp.Core.Config;

namespace devoctomy.Passchamp.Client;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        ApplyTheme();
        var appShell = MauiProgram.MauiApp.Services.GetService<AppShellPage>();
        MainPage = appShell;
    }

    private void ApplyTheme()
    {
        var configLoaderService = MauiProgram.MauiApp.Services.GetService<IApplicationConfigLoaderService<AppConfig>>();
        configLoaderService.LoadAsync(CancellationToken.None).GetAwaiter().GetResult();
        if (Enum.TryParse<AppTheme>(configLoaderService.Config.Theme, out var theme))
        {
            UserAppTheme = theme;
        }
        else
        {
            UserAppTheme = Application.Current.PlatformAppTheme;
        }
    }
}