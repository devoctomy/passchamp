﻿namespace devoctomy.Passchamp.Core.Cloud;

public interface ICloudStorageProviderConfig
{
    public string ProviderTypeId { get; }
    public string DisplayName { get; set; }
}
