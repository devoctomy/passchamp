using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Services;
using Microsoft.Extensions.DependencyInjection;

namespace devoctomy.Passchamp.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPasschampCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IGraphLoaderService, GraphLoaderService>();
            services.AddScoped<IPinsJsonParserService, PinsJsonParserService>();
            services.AddScoped<INodesJsonParserService, NodesJsonParserService>();
        }
    }
}
