using CommunityToolkit.Maui;
using devoctomy.Passchamp.Client.Pages;
using devoctomy.Passchamp.Client.ViewModels;
using devoctomy.Passchamp.Client.Views;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Extensions;
using devoctomy.Passchamp.Maui.Extensions;

namespace devoctomy.Passchamp.Client;

public static class MauiProgram
{
    public static MauiApp MauiApp { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddTransient(x => SecureStorage.Default);
        var passchampCoreServiceOptions = new PasschampCoreServicesOptions
        {
            CloudStorageProviderConfigLoaderServiceOptions = new Core.Cloud.CloudStorageProviderConfigLoaderServiceOptions
            {
                FileName = "providers.json",
                Path = Path.Combine(FileSystem.AppDataDirectory, $"config\\")
            }
        };
        builder.Services.AddPasschampCoreServices(passchampCoreServiceOptions);
        builder.Services.AddPasschampMauiServices(new PasschampMauiServicesOptions
        {
            VaultLoaderServiceOptions = new Maui.Data.VaultLoaderServiceOptions
            {
                FileName = "vaults.json",
                Path = Path.Combine(FileSystem.AppDataDirectory, $"vaults\\")
            },
            ShellNavigationServiceOptions = new Maui.Services.ShellNavigationServiceOptions
            {
                HomeRoute = "//Vaults"
            }
        });
        RegisterViewModels(builder.Services);
        RegisterPages(builder.Services);

        MauiApp = builder.Build();
        return MauiApp;
    }

    static void RegisterPages(IServiceCollection services)
    {
        services.AddTransient<AppShellPage>();
        services.AddTransient<VaultsPage>();
        services.AddTransient<SettingsPage>();
        services.AddTransient<CloudSettingsView>();
        services.AddTransient<CloudStorageProviderEditorPage>();
        services.AddTransient<VaultEditorPage>();
    }

    static void RegisterViewModels(IServiceCollection services)
    {
        services.AddTransient<AppShellViewModel>();
        services.AddTransient<VaultsViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<CloudSettingsViewModel>();
        services.AddTransient<CloudStorageProviderEditorViewModel>();
        services.AddTransient<VaultEditorViewModel>();
    }
}