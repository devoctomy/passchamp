using CommunityToolkit.Maui;
using devoctomy.Passchamp.Client.Config;
using devoctomy.Passchamp.Client.Pages;
using devoctomy.Passchamp.Client.ViewModels;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Client.Views;
using devoctomy.Passchamp.Core.Config;
using devoctomy.Passchamp.Core.Extensions;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Maui.Extensions;
using System.Diagnostics;

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
            VaultLoaderServiceOptions = new Maui.Data.VaultIndexLoaderServiceOptions
            {
                FileName = "vaults.json",
                Path = Path.Combine(FileSystem.AppDataDirectory, $"vaults\\")
            },
            ShellNavigationServiceOptions = new Maui.Services.ShellNavigationServiceOptions
            {
                HomeRoute = "//Vaults"
            },
            ThemeAwareImageResourceServiceOptions = new Maui.Services.ThemeAwareImageResourceServiceOptions
            {
                SupportedThemes = new[] { "light", "dark" }
            }
        });

        var applicationConfigLoaderOptions = new ApplicationConfigLoaderServiceOptions
        {
            FileName = "app.config",
            Path = Path.Combine(FileSystem.AppDataDirectory, $"config\\")
        };
        builder.Services.AddSingleton(applicationConfigLoaderOptions);
        builder.Services.AddTransient<IApplicationConfigLoaderService<AppConfig>, ApplicationConfigLoaderService<AppConfig>>();

        RegisterViewModels(builder.Services);
        RegisterPages(builder.Services);

        MauiApp = builder.Build();
        return MauiApp;
    }

    static void RegisterPages(IServiceCollection services)
    {
        services.AddTransient<AppShellPage>();
        services.AddTransient<VaultsPage>();
        services.AddTransient<VaultPage>();
        services.AddTransient<SettingsPage>();
        services.AddTransient<ThemeTestPage>();
        services.AddTransient<GeneralSettingsView>();
        services.AddTransient<CloudSettingsView>();
        services.AddTransient<CloudStorageProviderEditorPage>();
        services.AddTransient<VaultEditorPage>();
        services.AddTransient<EnterMasterPassphrasePage>();
    }

    static void RegisterViewModels(IServiceCollection services)
    {
        AddViewModelsOfType<BaseAppShellViewModel>(services);
        AddViewModelsOfType<BaseAppShellPageViewModel>(services);
        AddViewModelsOfType<BaseViewModel>(services);
    }

    private static void AddViewModelsOfType<T>(IServiceCollection services)
    {
        var baseViewModelAssembly = typeof(T).Assembly;
        var allViewModelTypes = baseViewModelAssembly.GetTypes().Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface).ToList();
        foreach (var viewModelType in allViewModelTypes)
        {
            // !!! Might want to look at this in the future, excluding if starts with Base, as one base model inherits from baseviewmodel which breaks IOC
            if(viewModelType.Name == typeof(T).Name || viewModelType.Name.StartsWith("base", StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            Debug.WriteLine($"Configuring view model type '{viewModelType.Name}/{typeof(T).Name}' for dependency injection.");
            services.AddTransient(viewModelType);
        }
    }
}