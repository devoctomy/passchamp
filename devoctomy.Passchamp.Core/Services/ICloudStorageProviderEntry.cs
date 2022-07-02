using System;

namespace devoctomy.Passchamp.Core.Services
{
    public interface ICloudStorageProviderEntry
    {
        public string Name { get; set; }
        public bool IsFolder { get; set; }
        public string Path { get; set; }
        public string Hash { get; set; }
        public DateTime LastModified { get; set; }
    }
}
