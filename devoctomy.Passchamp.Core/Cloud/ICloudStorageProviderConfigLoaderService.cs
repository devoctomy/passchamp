using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cloud
{
    public interface ICloudStorageProviderConfigLoaderService
    {
        public List<CloudStorageProviderConfigRef> Refs { get; }
        public Task Load(CancellationToken cancellationToken);
        public Task Save(CancellationToken cancellationToken);
    }
}
