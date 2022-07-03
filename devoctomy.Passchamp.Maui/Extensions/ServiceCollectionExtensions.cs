using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Maui.Data;

namespace devoctomy.Passchamp.Maui.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPasschampMauiServices(this IServiceCollection services)
        {
            services.AddScoped<ISecureSettingStorageService, SecureSettingStorageService>();
        }
    }
}
