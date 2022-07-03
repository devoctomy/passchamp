using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Data.Attributes;
using System;
using System.Text.Json.Serialization;

namespace devoctomy.Passchamp.Core.Cloud.AmazonS3
{
    public class AmazonS3CloudStorageProviderConfig : IPartiallySecure, ICloudStorageProviderConfig
    {
        public string Id { get; set; }
        public string ProviderTypeId => AmazonS3CloudStorageProviderService.ProviderTypeId;
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
