using System;

namespace devoctomy.Passchamp.Core.Cloud
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CloudStorageProviderServiceAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string TypeId { get; set; }
    }
}
