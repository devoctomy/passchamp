using CommunityToolkit.Maui;
using devoctomy.Passchamp.Client.Pages;
using devoctomy.Passchamp.Client.ViewModels;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Extensions;
using devoctomy.Passchamp.Maui.Extensions;

namespace devoctomy.Passchamp.Client
{
    public static class MauiProgram
    {
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
                }
            });
            RegisterViewModels(builder.Services);
            RegisterPages(builder.Services);

            return builder.Build();
        }

        static void RegisterPages(IServiceCollection services)
        {
            services.AddTransient<VaultsPage>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<CloudStorageProviderEditorPage>();
        }

        static void RegisterViewModels(IServiceCollection services)
        {
            services.AddTransient<VaultsViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<CloudStorageProviderEditorViewModel>();
        }
    }
}