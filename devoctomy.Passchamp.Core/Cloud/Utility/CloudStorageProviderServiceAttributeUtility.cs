using System;
using System.Collections.Generic;
using System.Linq;

namespace devoctomy.Passchamp.Core.Cloud.Utility;

public static class CloudStorageProviderServiceAttributeUtility
{
    private static Dictionary<Type, CloudStorageProviderServiceAttribute> _attributeCache;
    private static readonly object _lock = new();

    public static CloudStorageProviderServiceAttribute Get(string typeId)
    {
        CacheAll();

        return _attributeCache.Single(x => x.Value.ProviderTypeId == typeId).Value;
    }

    public static CloudStorageProviderServiceAttribute Get(Type type)
    {
        CacheAll();

        if (_attributeCache.ContainsKey(type))
        {
            return _attributeCache[type];
        }

        return null;
    }

    private static void CacheAll()
    {
        lock (_lock)
        {
            if (_attributeCache != null)
            {
                return;
            }

            var attributeCache = new Dictionary<Type, CloudStorageProviderServiceAttribute>();
            var assembly = typeof(ICloudStorageProviderService).Assembly;
            var providerServices = assembly.GetTypes().Where(x => typeof(ICloudStorageProviderService).IsAssignableFrom(x) && !x.IsInterface).ToList();
            foreach (var curProviderService in providerServices)
            {
                var attribute = (CloudStorageProviderServiceAttribute)curProviderService.GetCustomAttributes(typeof(CloudStorageProviderServiceAttribute), true).Single();
                attributeCache.Add(curProviderService, attribute);
            }
            _attributeCache = attributeCache;
        }
    }
}
