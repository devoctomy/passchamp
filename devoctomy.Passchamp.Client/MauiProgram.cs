using devoctomy.Passchamp.Client.Pages;
using devoctomy.Passchamp.Client.ViewModels;

namespace devoctomy.Passchamp.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            RegisterPages(builder.Services);
            RegisterViewModels(builder.Services);

            return builder.Build();
        }

        static void RegisterPages(in IServiceCollection services)
        {
            services.AddTransient<VaultsPage>();
            services.AddTransient<SettingsPage>();
        }

        static void RegisterViewModels(in IServiceCollection services)
        {
            services.AddTransient<VaultsViewModel>();
            services.AddTransient<SettingsViewModel>();
        }
    }
}