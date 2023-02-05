using devoctomy.Passchamp.Core.Cloud.Utility;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Data.Attributes;
using Newtonsoft.Json;
using System;

namespace devoctomy.Passchamp.Core.Cloud.AmazonS3;

public class AmazonS3CloudStorageProviderConfig : IPartiallySecure, ICloudStorageProviderConfig, ICloneable
{
    public string Id { get; set; }
    public string ProviderTypeId => CloudStorageProviderServiceAttributeUtility.Get(typeof(AmazonS3CloudStorageProviderService)).ProviderTypeId;
    public string DisplayName { get; set; }
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

    public object Clone()
    {
        var clone = new AmazonS3CloudStorageProviderConfig
        {
            Id = Id,
            DisplayName = DisplayName,
            AccessId = AccessId,
            SecretKey = SecretKey,
            Region = Region,
            Bucket = Bucket,
            Path = Path
        };
        return clone;
    }
}
