using devoctomy.Passchamp.Core.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cloud
{
    public interface ICloudStorageProviderConfigLoaderService
    {
        public List<CloudStorageProviderConfigRef> Refs { get; }
        public Task LoadAsync(CancellationToken cancellationToken);
        public Task<CloudStorageProviderConfigRef> Add<T>(
            T configuration,
            CancellationToken cancellationToken) where T : IPartiallySecure, ICloudStorageProviderConfig;
        public Task<T> UnpackConfigAsync<T>(
            string id,
            CancellationToken cancellationToken) where T : IPartiallySecure;
    }
}
