using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Exceptions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cloud;

public class CloudStorageProviderConfigLoaderService : ICloudStorageProviderConfigLoaderService
{
    public IReadOnlyList<CloudStorageProviderConfigRef> Refs => _refs;

    private readonly CloudStorageProviderConfigLoaderServiceOptions _options;
    private readonly IPartialSecureJsonReaderService _partialSecureJsonReaderService;
    private readonly IPartialSecureJsonWriterService _partialSecureJsonWriterService;
    private readonly IIOService _ioService;
    private List<CloudStorageProviderConfigRef> _refs;

    public CloudStorageProviderConfigLoaderService(
        CloudStorageProviderConfigLoaderServiceOptions options,
        IPartialSecureJsonReaderService partialSecureJsonReaderService,
        IPartialSecureJsonWriterService partialSecureJsonWriterService,
        IIOService ioService)
    {
        _options = options;
        _refs = new List<CloudStorageProviderConfigRef>();
        _partialSecureJsonReaderService = partialSecureJsonReaderService;
        _partialSecureJsonWriterService = partialSecureJsonWriterService;
        _ioService = ioService;
    }

    public async Task LoadAsync(CancellationToken cancellationToken)
    {
        var fullPath = $"{_options.Path}{_options.FileName}";
        _ioService.CreatePathDirectory(fullPath);
        if (_ioService.Exists(fullPath))
        {
            var jsonRaw = await _ioService.ReadAllTextAsync(
                fullPath,
                cancellationToken);
            _refs = JsonConvert.DeserializeObject<List<CloudStorageProviderConfigRef>>(jsonRaw);
        }
    }

    public async Task<CloudStorageProviderConfigRef> AddAsync<T>(
        T configuration,
        CancellationToken cancellationToken) where T : IPartiallySecure, ICloudStorageProviderConfig
    {
        await SaveConfigurationAsync(
            configuration,
            cancellationToken);

        var newRef = new CloudStorageProviderConfigRef
        {
            Id = configuration.Id,
            ProviderServiceTypeId = configuration.ProviderTypeId
        };
        _refs.Add(newRef);
        var jsonRaw = JsonConvert.SerializeObject(Refs);
        var refsFullPath = $"{_options.Path}{_options.FileName}"; 
        await _ioService.WriteDataAsync(
            refsFullPath,
            jsonRaw,
            cancellationToken);
        return newRef;
    }

    public async Task RemoveAsync(
        string id,
        CancellationToken cancellationToken)
    {
        var toRemove = Refs.SingleOrDefault(x => x.Id == id);
        if(toRemove != null)
        {
            _refs.Remove(toRemove);
            var jsonRaw = JsonConvert.SerializeObject(Refs);
            var refsFullPath = $"{_options.Path}{_options.FileName}";
            await _ioService.WriteDataAsync(
                refsFullPath,
                jsonRaw,
                cancellationToken);
            var configFullPath = $"{_options.Path}{id}.json";
            _partialSecureJsonWriterService.RemoveAll(toRemove);
            _ioService.Delete(configFullPath);
        }
    }

    public async Task UpdateAsync<T>(
        T update, 
        CancellationToken cancellationToken) where T : IPartiallySecure, ICloudStorageProviderConfig
    {
        var toUpdate = Refs.SingleOrDefault(x => x.Id == update.Id);
        if (toUpdate != null)
        {
            await SaveConfigurationAsync(
                update,
                cancellationToken);
        }
    }

    public Task<T> UnpackConfigAsync<T>(
        string id,
        CancellationToken cancellationToken) where T : IPartiallySecure
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

    private async Task SaveConfigurationAsync<T>(
        T configuration,
        CancellationToken cancellationToken) where T : IPartiallySecure, ICloudStorageProviderConfig
    {
        using var configData = new MemoryStream();
        await _partialSecureJsonWriterService.SaveAsync(
            configuration,
            configData);
        var configFullPath = $"{_options.Path}{configuration.Id}.json";
        using var output = _ioService.OpenNewWrite(configFullPath);
        configData.Seek(0, SeekOrigin.Begin);
        await configData.CopyToAsync(output, cancellationToken);
        await output.FlushAsync(cancellationToken);
        output.Close();
    }
}
