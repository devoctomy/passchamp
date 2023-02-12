using System;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cloud;

public class CloudStorageProviderServiceFactory : ICloudStorageProviderServiceFactory
{
    public Task<ICloudStorageProviderService> Create(string id)
    {
        throw new NotImplementedException();
    }
}
