using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cloud
{
    public interface ICloudStorageProviderServiceFactory
    {
        public Task<ICloudStorageProviderService> Create(string id);
    }
}