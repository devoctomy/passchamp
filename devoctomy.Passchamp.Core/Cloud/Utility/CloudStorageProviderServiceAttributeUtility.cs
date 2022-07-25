using System;
using System.Collections.Generic;
using System.Linq;

namespace devoctomy.Passchamp.Core.Cloud.Utility
{
    public class CloudStorageProviderServiceAttributeUtility
    {
        private static Dictionary<Type, CloudStorageProviderServiceAttribute> _attributeCache;
        private static readonly object _lock = new object();

        public static CloudStorageProviderServiceAttribute Get(string typeId)
        {
            return _attributeCache.Single(x => x.Value.TypeId == typeId).Value;
        }

        public static CloudStorageProviderServiceAttribute Get<T>() where T : ICloudStorageProviderService
        {
            return Get(typeof(T));
        }

        public static CloudStorageProviderServiceAttribute Get(Type type)
        {
            lock(_lock)
            {
                if(_attributeCache == null)
                {
                    CacheAll();
                }
            }

            if (_attributeCache.ContainsKey(type))
            {
                return _attributeCache[type];
            }

            var attribute = (CloudStorageProviderServiceAttribute)type.GetCustomAttributes(typeof(CloudStorageProviderServiceAttribute), true).Single();
            _attributeCache.Add(type, attribute);
            return attribute;
        }

        private static void CacheAll()
        {
            _attributeCache = new Dictionary<Type, CloudStorageProviderServiceAttribute>();
            var assembly = typeof(ICloudStorageProviderService).Assembly;
            var providerServices = assembly.GetTypes().Where(x => typeof(ICloudStorageProviderService).IsAssignableFrom(x) && !x.IsInterface).ToList();
            foreach (var curProviderService in providerServices)
            {
                Get(curProviderService);
            }
        }
    }
}
