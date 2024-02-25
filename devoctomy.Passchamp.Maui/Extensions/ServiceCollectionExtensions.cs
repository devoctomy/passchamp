using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Maui.Data;
using devoctomy.Passchamp.Maui.IO;
using devoctomy.Passchamp.Maui.Services;

namespace devoctomy.Passchamp.Maui.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPasschampMauiServices(
        this IServiceCollection services,
        PasschampMauiServicesOptions options)
    {
        services.AddSingleton(options.VaultLoaderServiceOptions);
        services.AddSingleton(options.ShellNavigationServiceOptions);
        services.AddSingleton(options.ThemeAwareImageResourceServiceOptions);

        services.AddSingleton<IThemeAwareImageResourceService, ThemeAwareImageResourceService>();
        services.AddSingleton<IShellNavigationService, ShellNavigationService>();

        services.AddScoped<ISecureSettingStorageService, SecureSettingStorageService>();
        services.AddScoped<IVaultLoaderService, VaultLoaderService>();
        services.AddScoped<IVaultCreatorService, VaultCreatorService>();

        services.AddTransient<IXamlHelperService, XamlHelperService>();

        //Platform specific
        services.AddSingleton<IPathResolverService>(serviceProvider =>
        {
#if ANDROID
            return new devoctomy.Passchamp.Maui.Pathforms.Android.IO.PathResolver();
#elif WINDOWS
            return new devoctomy.Passchamp.Maui.Pathforms.Android.IO.PathResolver();
#else
            throw new PlatformNotSupportedException();
#endif
        });
    }
}
