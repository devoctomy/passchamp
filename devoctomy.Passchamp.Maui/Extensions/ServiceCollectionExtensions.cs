namespace devoctomy.Passchamp.Maui.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPasschampMauiServices(this IServiceCollection services)
        {
            services.AddScoped<IPartialSecureJsonReader, PartialSecureJsonReader>();
            services.AddScoped<IPartialSecureJsonWriter, PartialSecureJsonWriter>();
            services.AddScoped<ISecureSettingStorageService, SecureSettingStorageService>();
        }
    }
}
