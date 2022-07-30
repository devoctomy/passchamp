using devoctomy.Passchamp.Core.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cloud
{
    public interface ICloudStorageProviderConfigLoaderService
    {
        public IReadOnlyList<CloudStorageProviderConfigRef> Refs { get; }
        public Task LoadAsync(CancellationToken cancellationToken);
        public Task<CloudStorageProviderConfigRef> AddAsync<T>(
            T configuration,
            CancellationToken cancellationToken) where T : IPartiallySecure, ICloudStorageProviderConfig;
        public Task RemoveAsync(
            string id,
            CancellationToken cancellationToken);
        public Task<T> UnpackConfigAsync<T>(
            string id,
            CancellationToken cancellationToken) where T : IPartiallySecure;
    }
}
