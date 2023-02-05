using Amazon.S3;
using Amazon.S3.Model;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cloud.AmazonS3;

[CloudStorageProviderServiceAttribute(
    ProviderTypeId = "76EEB72B-28DB-49E5-BE25-A2B625BAB333",
    DisplayName = "Amazon S3 Cloud Storage Provider")]
public class AmazonS3CloudStorageProviderService : ICloudStorageProviderService
{
    public string DisplayName => "";
    public string TypeId => "";

    private readonly IAmazonS3Config _config;
    private readonly IAmazonS3 _amazonS3;

    public AmazonS3CloudStorageProviderService(
        IAmazonS3Config config,
        IAmazonS3 amazonS3)
    {
        _config = config;
        _amazonS3 = amazonS3;
    }

    public async Task<CloudStorageProviderObjectResponse<ICloudStorageProviderEntry>> GetFileInfoAsync(
        string path,
        CancellationToken cancellationToken)
    {
        try
        {
            var fullPath = $"{_config.Path}{path}";
            var request = new GetObjectMetadataRequest
            {
                BucketName = _config.Bucket,
                Key = path
            };

            var response = await _amazonS3.GetObjectMetadataAsync(
                request,
                cancellationToken);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var file = new AmazonS3CloudStorageProviderEntry(
                    path,
                    false,
                    fullPath,
                    response.ETag.ToUpper(),
                    response.LastModified);
                return new CloudStorageProviderObjectResponse<ICloudStorageProviderEntry>(
                    true,
                    System.Net.HttpStatusCode.OK,
                    file);
            }

            return new CloudStorageProviderObjectResponse<ICloudStorageProviderEntry>(
                false,
                response.HttpStatusCode,
                null);
        }
        catch (AmazonS3Exception)
        {
            return new CloudStorageProviderObjectResponse<ICloudStorageProviderEntry>(
                false,
                null,
                null);
        }
    }

    public async Task<CloudStorageProviderObjectResponse<List<ICloudStorageProviderEntry>>> ListFilesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var request = new ListObjectsRequest
            {
                Prefix = $"{_config.Path}/",
                BucketName = _config.Bucket
            };
            var response = await _amazonS3.ListObjectsAsync(
                request,
                cancellationToken);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return new CloudStorageProviderObjectResponse<List<ICloudStorageProviderEntry>>(
                    true,
                    System.Net.HttpStatusCode.OK,
                    GetEntriesFromResponse(response));
            }

            return new CloudStorageProviderObjectResponse<List<ICloudStorageProviderEntry>>(
                false,
                response.HttpStatusCode,
                null);
        }
        catch (AmazonS3Exception)
        {
            return new CloudStorageProviderObjectResponse<List<ICloudStorageProviderEntry>>(
                false,
                null,
                null);
        }
    }

    public async Task<CloudStorageProviderResponse> PutFileAsync(
        Stream data,
        string path,
        CancellationToken cancellationToken)
    {
        try
        {
            var fullPath = $"{_config.Path}{path}";
            var request = new PutObjectRequest()
            {
                AutoCloseStream = true,
                AutoResetStreamPosition = true,
                InputStream = data,
                Key = fullPath,
                BucketName = _config.Bucket
            };
            var response = await _amazonS3.PutObjectAsync(
                request,
                cancellationToken);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return new CloudStorageProviderResponse(
                    true,
                    System.Net.HttpStatusCode.OK);
            }

            return new CloudStorageProviderResponse(
                false,
                response.HttpStatusCode);
        }
        catch (AmazonS3Exception)
        {
            return new CloudStorageProviderResponse(
                false,
                null);
        }
    }

    private static List<ICloudStorageProviderEntry> GetEntriesFromResponse(ListObjectsResponse response)
    {
        var files = new List<ICloudStorageProviderEntry>();
        foreach (S3Object curObject in response.S3Objects)
        {
            var isFolder = curObject.Key.EndsWith("/");
            string name;
            if (isFolder)
            {
                var removedTrailing = curObject.Key.TrimEnd('/');
                name = removedTrailing[(removedTrailing.LastIndexOf("/") + 1)..];
            }
            else
            {
                name = curObject.Key[curObject.Key.LastIndexOf("/")..];
            }

            files.Add(new AmazonS3CloudStorageProviderEntry(
                isFolder ? name : name.TrimStart('/'),
                isFolder,
                isFolder ? curObject.Key.TrimEnd('/') : curObject.Key,
                isFolder ? null : curObject.ETag.ToUpper(),
                curObject.LastModified));
        }

        return files;
    }
}
