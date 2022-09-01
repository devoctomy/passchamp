namespace devoctomy.Passchamp.Client;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        var appShell = MauiProgram.MauiApp.Services.GetService<AppShellPage>();
        MainPage = appShell;
    }
}