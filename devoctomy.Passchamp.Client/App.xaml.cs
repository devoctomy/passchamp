using devoctomy.Passchamp.Client.Config;
using devoctomy.Passchamp.Core.Config;
using devoctomy.Passchamp.Maui.IO;

namespace devoctomy.Passchamp.Client;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        ApplyTheme();
        Initialise();
        var appShell = MauiProgram.MauiApp.Services.GetService<AppShellPage>();
        MainPage = appShell;
    }

    private void Initialise()
    {
#if ANDROID
        var pathResolver = (devoctomy.Passchamp.Maui.Pathforms.Android.IO.PathResolver)MauiProgram.MauiApp.Services.GetService<IPathResolver>();
        pathResolver.Initialise(this.Handler.MauiContext);
#endif
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