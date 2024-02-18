using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Cryptography;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Presets;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace devoctomy.Passchamp.Core.Extensions;

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
        services.AddSingleton<ICloudStorageProviderConfigLoaderService, CloudStorageProviderConfigLoaderService>();
        services.AddSingleton<IGraphFactory, GraphFactory>();

        AddAllOfType<IGraphPinPrepFunction>(services);
        AddAllOfType<IGraphPinOutputFunction>(services);
        AddAllOfType<IGraphPreset>(services);
        AddAllOfType<IGraphPresetSet>(services);

        AddNodes(services);
    }

    private static void AddAllOfType<T>(IServiceCollection services)
    {
        var assembly = typeof(T).Assembly;
        var allTypes = assembly.GetTypes().Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface).ToList();
        foreach (var curType in allTypes)
        {
            services.AddScoped(typeof(T), curType);
        }
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
