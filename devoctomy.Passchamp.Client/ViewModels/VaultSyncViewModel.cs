using CommunityToolkit.Mvvm.ComponentModel;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Cloud;
using System.Collections.ObjectModel;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class VaultSyncViewModel : BaseViewModel
{
    [ObservableProperty]
    private string fileName;

    [ObservableProperty]
    private CloudStorageProviderConfigRef cloudStorageProviderConfigRef;

    [ObservableProperty]
    private ObservableCollection<CloudStorageProviderConfigRef> cloudStorageProviderConfigRefs;

    private readonly ICloudStorageProviderConfigLoaderService _cloudStorageProviderConfigLoaderService;

    public VaultSyncViewModel(ICloudStorageProviderConfigLoaderService cloudStorageProviderConfigLoaderService)
    {
        _cloudStorageProviderConfigLoaderService = cloudStorageProviderConfigLoaderService;
    }

    public override async Task Init()
    {
        await _cloudStorageProviderConfigLoaderService.LoadAsync(CancellationToken.None);
        CloudStorageProviderConfigRefs = new ObservableCollection<CloudStorageProviderConfigRef>(_cloudStorageProviderConfigLoaderService.Refs);

        return;
    }

}
