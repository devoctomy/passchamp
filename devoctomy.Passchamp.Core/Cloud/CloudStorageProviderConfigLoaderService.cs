using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cloud
{
    public class CloudStorageProviderConfigLoaderService : ICloudStorageProviderConfigLoaderService
    {
        private readonly CloudStorageProviderConfigLoaderServiceOptions _options;

        public List<CloudStorageProviderConfigRef> Refs { get; private set; }

        public CloudStorageProviderConfigLoaderService(CloudStorageProviderConfigLoaderServiceOptions options)
        {
            _options = options;
            Refs = new List<CloudStorageProviderConfigRef>();
        }

        public async Task Load(CancellationToken cancellationToken)
        {
            var jsonRaw = await File.ReadAllTextAsync(
                _options.ConfigPath,
                cancellationToken);
            Refs = JsonConvert.DeserializeObject<List<CloudStorageProviderConfigRef>>(jsonRaw);
        }

        public async Task Save(CancellationToken cancellationToken)
        {
            var jsonRaw = JsonConvert.SerializeObject(Refs);
            await File.WriteAllTextAsync(
                _options.ConfigPath,
                jsonRaw,
                cancellationToken);
        }
    }
}
