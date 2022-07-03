using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Exceptions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cloud
{
    public class CloudStorageProviderConfigLoaderService : ICloudStorageProviderConfigLoaderService
    {
        public List<CloudStorageProviderConfigRef> Refs { get; private set; }

        private readonly CloudStorageProviderConfigLoaderServiceOptions _options;
        private readonly IPartialSecureJsonReaderService _partialSecureJsonReaderService;
        private readonly IPartialSecureJsonWriterService _partialSecureJsonWriterService;
        private readonly IIOService _ioService;

        public CloudStorageProviderConfigLoaderService(
            CloudStorageProviderConfigLoaderServiceOptions options,
            IPartialSecureJsonReaderService partialSecureJsonReaderService,
            IPartialSecureJsonWriterService partialSecureJsonWriterService,
            IIOService ioService)
        {
            _options = options;
            Refs = new List<CloudStorageProviderConfigRef>();
            _partialSecureJsonReaderService = partialSecureJsonReaderService;
            _partialSecureJsonWriterService = partialSecureJsonWriterService;
            _ioService = ioService;
        }

        public async Task LoadAsync(CancellationToken cancellationToken)
        {
            var fullPath = $"{_options.Path}{_options.FileName}";
            var jsonRaw = await _ioService.ReadAllTextAsync(
                fullPath,
                cancellationToken);
            Refs = JsonConvert.DeserializeObject<List<CloudStorageProviderConfigRef>>(jsonRaw);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            var fullPath = $"{_options.Path}{_options.FileName}";
            var jsonRaw = JsonConvert.SerializeObject(Refs);
            await _ioService.WriteDataAsync(
                fullPath,
                jsonRaw,
                cancellationToken);
        }

        public Task<T> UnpackConfigAsync<T>(
            string id,
            CancellationToken cancellationToken)
        {
            if(!Refs.Any(x => x.Id == id))
            {
                throw new UnknownCloudStorageProviderConfigIdException(id);
            }

            var fullPath = $"{_options.Path}{id}.json";
            var stream = _ioService.OpenRead(fullPath);
            var config = _partialSecureJsonReaderService.LoadAsync<T>(stream);
            return config;
        }
    }
}
