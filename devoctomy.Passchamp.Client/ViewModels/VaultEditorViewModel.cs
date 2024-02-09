using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Cloud;
using System.Collections.ObjectModel;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class VaultEditorViewModel : BaseViewModel
{
    [ObservableProperty]
    ObservableCollection<CloudStorageProviderConfigRef> cloudStorageProviderConfigRefs;

    public BaseViewModel ReturnViewModel { get; set; }
    public VaultInfoViewModel InfoViewModel { get; set; } = new VaultInfoViewModel(); // !!!
    public VaultSecurityViewModel SecurityViewModel { get; set; } = new VaultSecurityViewModel(); // !!!
    public IAsyncRelayCommand BackCommand { get; private set; }
    public IAsyncRelayCommand OkCommand { get; private set; }

    private readonly ICloudStorageProviderConfigLoaderService _cloudStorageProviderConfigLoaderService;

    public VaultEditorViewModel(
        BaseViewModel returnViewModel,
        ICloudStorageProviderConfigLoaderService cloudStorageProviderConfigLoaderService)
    {
        AttachCommandHandlers();

        ReturnViewModel = returnViewModel;
        _cloudStorageProviderConfigLoaderService = cloudStorageProviderConfigLoaderService;
    }

    public async Task Init()
    {
        await _cloudStorageProviderConfigLoaderService.LoadAsync(CancellationToken.None);
        CloudStorageProviderConfigRefs = new ObservableCollection<CloudStorageProviderConfigRef>(_cloudStorageProviderConfigLoaderService.Refs);
    }

    private void AttachCommandHandlers()
    {
        BackCommand = new AsyncRelayCommand(BackCommandHandler);
        OkCommand = new AsyncRelayCommand(OkCommandHandler);
    }

    private async Task BackCommandHandler()
    {
        await ReturnViewModel.Return(null);
    }

    private async Task OkCommandHandler()
    {
        await ReturnViewModel.Return(this);
    }
}
