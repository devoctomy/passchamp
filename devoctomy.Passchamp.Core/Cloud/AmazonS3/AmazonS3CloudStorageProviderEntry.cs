using System;

namespace devoctomy.Passchamp.Core.Cloud.AmazonS3
{
    public class AmazonS3CloudStorageProviderEntry : ICloudStorageProviderEntry
    {
        public string Name { get; set; }
        public bool IsFolder { get; set; }
        public string Path { get; set; }
        public string Hash { get; set; }
        public DateTime LastModified { get; set; }

        public AmazonS3CloudStorageProviderEntry(
            string name,
            bool isFolder,
            string path,
            string hash,
            DateTime lastModified)
        {
            Name = name;
            IsFolder = isFolder;
            Path = path;
            Hash = hash;
            LastModified = lastModified;
        }
    }
}
