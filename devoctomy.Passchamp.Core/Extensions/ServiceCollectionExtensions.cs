﻿using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace devoctomy.Passchamp.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPasschampCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IGraphLoaderService, GraphLoaderService>();
            services.AddScoped<INodesJsonParserService, NodesJsonParserService>();
            services.AddScoped<IPinsJsonParserService, PinsJsonParserService>();
            services.AddScoped<IDataParserSectionParser, DataParserSectionParser>();

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
                services.AddScoped(typeof(INode), node);
                services.AddScoped(node);
            }
        }
    }
}
