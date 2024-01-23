using devoctomy.Passchamp.Core.Data;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace devoctomy.Passchamp.Core.Config;

public class ApplicationConfigLoaderService<T> : IApplicationConfigLoaderService where T : IPartiallySecure
{
    public T Config { get; private set; } = Activator.CreateInstance<T>();

    private readonly ApplicationConfigLoaderServiceOptions _options;
    private readonly IPartialSecureJsonReaderService _partialSecureJsonReaderService;
    private readonly IPartialSecureJsonWriterService _partialSecureJsonWriterService;
    private readonly IIOService _ioService;

    public ApplicationConfigLoaderService(
        ApplicationConfigLoaderServiceOptions options,
        IPartialSecureJsonReaderService partialSecureJsonReaderService,
        IPartialSecureJsonWriterService partialSecureJsonWriterService,
        IIOService ioService)
    {
        _options = options;
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
            using var stream = _ioService.OpenRead(fullPath);
            Config = await _partialSecureJsonReaderService.LoadAsync<T>(stream);
        }
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        var fullPath = $"{_options.Path}{_options.FileName}";
        _ioService.CreatePathDirectory(fullPath);

        using var stream = _ioService.OpenNewWrite(fullPath);
        await _partialSecureJsonWriterService.SaveAsync(Config, stream);
    }
}
