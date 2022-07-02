using devoctomy.Passchamp.Maui.Services;

namespace devoctomy.Passchamp.Maui.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPasschampMauiServices(this IServiceCollection services)
        {
            services.AddScoped<IPartialSecureJsonReaderService, PartialSecureJsonReaderService>();
            services.AddScoped<IPartialSecureJsonWriterService, PartialSecureJsonWriterService>();
            services.AddScoped<ISecureSettingStorageService, SecureSettingStorageService>();
        }
    }
}
