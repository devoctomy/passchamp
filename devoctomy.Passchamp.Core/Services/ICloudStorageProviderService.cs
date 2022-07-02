using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Services
{
    public interface ICloudStorageProviderService
    {
        public Task<CloudProviderObjectResponse<ICloudStorageProviderEntry>> GetFileInfoAsync(
            string path,
            CancellationToken cancellationToken);

        public Task<CloudProviderResponse> PutFileAsync(
            Stream data,
            string path,
            CancellationToken cancellationToken);

        public Task<CloudProviderObjectResponse<List<ICloudStorageProviderEntry>>> ListFilesAsync(CancellationToken cancellationToken);
    }
}
