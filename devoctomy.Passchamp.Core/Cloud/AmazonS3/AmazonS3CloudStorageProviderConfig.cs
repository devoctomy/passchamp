using devoctomy.Passchamp.Core.Cloud.Utility;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Data.Attributes;
using System;
using System.Text.Json.Serialization;

namespace devoctomy.Passchamp.Core.Cloud.AmazonS3
{
    public class AmazonS3CloudStorageProviderConfig : IPartiallySecure, ICloudStorageProviderConfig
    {
        public string Id { get; set; }
        public string ProviderTypeId => CloudStorageProviderServiceAttributeUtility.Get(typeof(AmazonS3CloudStorageProviderService)).TypeId;
        public string AccessId { get; set; }
        [SecureSetting]
        [JsonIgnore]
        public string SecretKey { get; set; }
        public string Region { get; set; }
        public string Bucket { get; set; }
        public string Path { get; set; }

        public AmazonS3CloudStorageProviderConfig()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
