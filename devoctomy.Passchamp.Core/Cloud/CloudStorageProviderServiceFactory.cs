using System;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cloud
{
    public class CloudStorageProviderServiceFactory : ICloudStorageProviderServiceFactory
    {
        public CloudStorageProviderServiceFactory()
        {
            // we need to inject all stored cloud provider configurations here
        }

        public Task<ICloudStorageProviderService> Create(string id)
        {
            throw new NotImplementedException();
        }
    }
}
