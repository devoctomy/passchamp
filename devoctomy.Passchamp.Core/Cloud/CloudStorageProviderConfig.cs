using devoctomy.Passchamp.Core.Data;
using System;

namespace devoctomy.Passchamp.Core.Cloud
{
    public class CloudStorageProviderConfig : ICloudStorageProviderConfig, IPartiallySecure
    {
        public string Id { get; set; }
        public string ProviderTypeId { get; set; }
        public string DisplayName { get; set; }
    }
}
