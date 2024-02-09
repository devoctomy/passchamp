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
    public VaultInfoViewModel InfoViewModel { get; set; }
    public VaultSecurityViewModel SecurityViewModel { get; set; }
    public VaultSyncViewModel SyncViewModel { get; set; }
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

    public override async Task Init()
    {
        InfoViewModel = MauiProgram.MauiApp.Services.GetService<VaultInfoViewModel>();
        SecurityViewModel = MauiProgram.MauiApp.Services.GetService<VaultSecurityViewModel>();
        SyncViewModel = MauiProgram.MauiApp.Services.GetService<VaultSyncViewModel>();

        await SyncViewModel.Init();
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
