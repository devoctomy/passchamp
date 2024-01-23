namespace devoctomy.Passchamp.Client;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        UserAppTheme = Application.Current.PlatformAppTheme;    // Follow system theme
        var appShell = MauiProgram.MauiApp.Services.GetService<AppShellPage>();
        MainPage = appShell;
    }
}