using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cloud
{
    public interface ICloudStorageProviderConfigLoaderService
    {
        public List<CloudStorageProviderConfigRef> Refs { get; }
        public Task LoadAsync(CancellationToken cancellationToken);
        public Task SaveAsync(CancellationToken cancellationToken);
        public Task<T> UnpackConfigAsync<T>(
            string id,
            CancellationToken cancellationToken);
    }
}
