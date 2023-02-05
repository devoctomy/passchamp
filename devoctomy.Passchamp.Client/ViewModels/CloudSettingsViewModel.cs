using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Cloud.AmazonS3;
using System.Collections.ObjectModel;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class CloudSettingsViewModel : BaseViewModel
{
    [ObservableProperty]
    ObservableCollection<CloudStorageProviderConfigRef> cloudStorageProviderConfigRefs;

    [ObservableProperty]
    CloudStorageProviderConfigRef selectedCloudStorageProviderConfigRef;

    public IAsyncRelayCommand AddCloudStorageProviderCommand { get; }
    public IAsyncRelayCommand EditSelectedCloudStorageProviderCommand { get; }
    public IAsyncRelayCommand RemoveSelectedCloudStorageProviderCommand { get; }

    [ObservableProperty]
    private bool removeSelectedCloudStorageProviderCommandCanExecute;

    private readonly ICloudStorageProviderConfigLoaderService _cloudStorageProviderConfigLoaderService;

    public CloudSettingsViewModel(ICloudStorageProviderConfigLoaderService cloudStorageProviderConfigLoaderService)
    {
        AddCloudStorageProviderCommand = new AsyncRelayCommand(AddCloudStorageProviderCommandHandler);
        EditSelectedCloudStorageProviderCommand = new AsyncRelayCommand(EditCloudStorageProviderCommandHandler);
        RemoveSelectedCloudStorageProviderCommand = new AsyncRelayCommand(RemoveSelectedCloudStorageProviderHandler);
        removeSelectedCloudStorageProviderCommandCanExecute = false;
        _cloudStorageProviderConfigLoaderService = cloudStorageProviderConfigLoaderService;
    }

    public async Task Init()
    {
        await _cloudStorageProviderConfigLoaderService.LoadAsync(CancellationToken.None);
        CloudStorageProviderConfigRefs = new ObservableCollection<CloudStorageProviderConfigRef>(_cloudStorageProviderConfigLoaderService.Refs);
    }

    public override async Task Return(BaseViewModel viewModel)
    {
        await Application.Current.MainPage.Navigation.PopModalAsync();

        if (viewModel == null)
        {
            return;
        }

        if (viewModel is CloudStorageProviderEditorViewModel)
        {
            var cloudStorageProviderEditorViewModel = viewModel as CloudStorageProviderEditorViewModel;
            switch (cloudStorageProviderEditorViewModel.EditorMode)
            {
                case Enums.PageEditorMode.Create:
                    {
                        await CreateCloudStorageProvider(cloudStorageProviderEditorViewModel);
                        break;
                    }

                case Enums.PageEditorMode.Edit:
                    {
                        await UpdateCloudStorageProvider(cloudStorageProviderEditorViewModel);
                        break;
                    }

                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
        }
    }

    private async Task CreateCloudStorageProvider(CloudStorageProviderEditorViewModel model)
    {
        var config = new AmazonS3CloudStorageProviderConfig
        {
            DisplayName = model.DisplayName,
            AccessId = model.AccessId,
            SecretKey = model.SecretKey,
            Region = model.Region,
            Bucket = model.Bucket,
            Path = model.Path
        };
        var newRef = await _cloudStorageProviderConfigLoaderService.AddAsync(
            config,
            CancellationToken.None);
        CloudStorageProviderConfigRefs.Add(newRef);
    }

    private async Task UpdateCloudStorageProvider(CloudStorageProviderEditorViewModel model)
    {
        var update = new AmazonS3CloudStorageProviderConfig
        {
            Id = model.Id,
            DisplayName = model.DisplayName,
            AccessId = model.AccessId,
            SecretKey = model.SecretKey,
            Region = model.Region,
            Bucket = model.Bucket,
            Path = model.Path
        };
        await _cloudStorageProviderConfigLoaderService.UpdateAsync(
            update,
            CancellationToken.None);
    }

    private async Task AddCloudStorageProviderCommandHandler()
    {
        var viewModel = new CloudStorageProviderEditorViewModel(this);
        var page = new Pages.CloudStorageProviderEditorPage(viewModel);
        await Application.Current.MainPage.Navigation.PushModalAsync(page, true);
    }

    private async Task EditCloudStorageProviderCommandHandler()
    {
        if (SelectedCloudStorageProviderConfigRef == null)
        {
            return;
        }

        var config = await _cloudStorageProviderConfigLoaderService.UnpackConfigAsync<AmazonS3CloudStorageProviderConfig>(
            SelectedCloudStorageProviderConfigRef.Id,
            CancellationToken.None);
        config = (AmazonS3CloudStorageProviderConfig)config.Clone();

        var viewModel = new CloudStorageProviderEditorViewModel(
            config,
            this);
        var page = new Pages.CloudStorageProviderEditorPage(viewModel);
        await Application.Current.MainPage.Navigation.PushModalAsync(page, true);
    }

    private async Task RemoveSelectedCloudStorageProviderHandler()
    {
        if (SelectedCloudStorageProviderConfigRef == null)
        {
            return;
        }

        var selected = SelectedCloudStorageProviderConfigRef;
        CloudStorageProviderConfigRefs.Remove(selected);
        await _cloudStorageProviderConfigLoaderService.RemoveAsync(
            selected.Id,
            CancellationToken.None);
        SelectedCloudStorageProviderConfigRef = null;
    }
}
