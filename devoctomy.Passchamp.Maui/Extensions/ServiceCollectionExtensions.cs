using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Maui.Data;
using devoctomy.Passchamp.Maui.Services;

namespace devoctomy.Passchamp.Maui.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPasschampMauiServices(
            this IServiceCollection services,
            PasschampMauiServicesOptions options)
        {
            services.AddSingleton(options.VaultLoaderServiceOptions);
            services.AddSingleton(options.ShellNavigationServiceOptions);

            services.AddSingleton<IShellNavigationService, ShellNavigationService>();

            services.AddScoped<ISecureSettingStorageService, SecureSettingStorageService>();
            services.AddScoped<IVaultLoaderService, VaultLoaderService>();
        }
    }
}
