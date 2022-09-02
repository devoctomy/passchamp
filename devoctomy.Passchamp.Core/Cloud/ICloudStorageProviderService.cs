using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cloud;

public interface ICloudStorageProviderService
{
    public string DisplayName { get; }
    public string TypeId { get; }

    public Task<CloudStorageProviderObjectResponse<ICloudStorageProviderEntry>> GetFileInfoAsync(
        string path,
        CancellationToken cancellationToken);
    public Task<CloudStorageProviderResponse> PutFileAsync(
        Stream data,
        string path,
        CancellationToken cancellationToken);
    public Task<CloudStorageProviderObjectResponse<List<ICloudStorageProviderEntry>>> ListFilesAsync(CancellationToken cancellationToken);
}
