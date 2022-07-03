using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Cryptography;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace devoctomy.Passchamp.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPasschampCoreServices(
            this IServiceCollection services,
            PasschampCoreServicesOptions options)
        {
            services.AddSingleton(options.CloudStorageProviderConfigLoaderServiceOptions);

            services.AddScoped<ITypeResolverService, TypeResolverService>();
            services.AddScoped<IGraphLoaderService, GraphLoaderService>();
            services.AddScoped<INodesJsonParserService, NodesJsonParserService>();
            services.AddScoped<IInputPinsJsonParserService, InputPinsJsonParserService>();
            services.AddScoped<IOutputPinsJsonParserService, OutputPinsJsonParserService>();
            services.AddScoped<IDataParserSectionParser, DataParserSectionParser>();
            services.AddScoped<ISecureStringUnpacker, SecureStringUnpacker>();
            services.AddScoped<IPartialSecureJsonReaderService, PartialSecureJsonReaderService>();
            services.AddScoped<IPartialSecureJsonWriterService, PartialSecureJsonWriterService>();
            services.AddScoped<IIOService, IOService>();
            services.AddScoped<ICloudStorageProviderConfigLoaderService, CloudStorageProviderConfigLoaderService>();

            var pinPrepFunctionAssembly = typeof(IGraphPinPrepFunction).Assembly;
            var allPinPrepFunctions = pinPrepFunctionAssembly.GetTypes().Where(x => typeof(IGraphPinPrepFunction).IsAssignableFrom(x) && !x.IsInterface).ToList();
            foreach(var pinPrepFunction in allPinPrepFunctions)
            {
                services.AddScoped(typeof(IGraphPinPrepFunction), pinPrepFunction);
            }

            var pinOutputFunctionAssembly = typeof(IGraphPinOutputFunction).Assembly;
            var allPinOutputFunctions = pinOutputFunctionAssembly.GetTypes().Where(x => typeof(IGraphPinOutputFunction).IsAssignableFrom(x) && !x.IsInterface).ToList();
            foreach (var pinOutputFunction in allPinOutputFunctions)
            {
                services.AddScoped(typeof(IGraphPinOutputFunction), pinOutputFunction);
            }

            AddNodes(services);
        }

        private static void AddNodes(this IServiceCollection services)
        {
            var nodeAssembly = typeof(INode).Assembly;
            var allNodes = nodeAssembly.GetTypes().Where(x => typeof(INode).IsAssignableFrom(x) && !x.IsInterface).ToList();
            foreach (var node in allNodes)
            {
                services.AddTransient(typeof(INode), node);
                services.AddTransient(node);
            }
        }
    }
}
