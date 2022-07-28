using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Cloud.AmazonS3;
using devoctomy.Passchamp.Core.Cloud.Utility;
using devoctomy.Passchamp.Core.Data;
using System.Collections.ObjectModel;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<CloudStorageProviderConfigRef> cloudStorageProviderConfigRefs; //{ get; set; } = new ObservableCollection<CloudStorageProviderConfigRef>();

        [ObservableProperty]
        CloudStorageProviderConfigRef selectedCloudStorageProviderConfigRef;

        public IAsyncRelayCommand AddCloudStorageProviderCommand { get; }
        public IAsyncRelayCommand RemoveSelectedCloudStorageProviderCommand { get; }

        [ObservableProperty]
        private bool removeSelectedCloudStorageProviderCommandCanExecute;

        private readonly ICloudStorageProviderConfigLoaderService _cloudStorageProviderConfigLoaderService;
        private static SemaphoreSlim _loaderLock = new SemaphoreSlim(1, 1);
        private bool _loaded = false;

        public SettingsViewModel(ICloudStorageProviderConfigLoaderService cloudStorageProviderConfigLoaderService)
        {
            AddCloudStorageProviderCommand = new AsyncRelayCommand(AddCloudStorageProviderCommandHandler);
            RemoveSelectedCloudStorageProviderCommand = new AsyncRelayCommand(RemoveSelectedCloudStorageProviderHandler);
            removeSelectedCloudStorageProviderCommandCanExecute = false;
            _cloudStorageProviderConfigLoaderService = cloudStorageProviderConfigLoaderService;
        }

        public override async Task OnAppearingAsync()
        {
            await _loaderLock.WaitAsync();
            try
            {
                if (!_loaded)
                {
                    await _cloudStorageProviderConfigLoaderService.LoadAsync(CancellationToken.None);
                    CloudStorageProviderConfigRefs = new ObservableCollection<CloudStorageProviderConfigRef>(_cloudStorageProviderConfigLoaderService.Refs);
                    _loaded = true;
                }
            }
            finally
            {
                _loaderLock.Release();
            }  
        }

        public override async Task Return(BaseViewModel viewModel)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();

            if (viewModel is CloudStorageProviderEditorViewModel)
            {
                var cloudStorageProviderEditorViewModel = viewModel as CloudStorageProviderEditorViewModel;
                switch(cloudStorageProviderEditorViewModel.EditorMode)
                {
                    case Enums.PageEditorMode.Create:
                        {
                            await CreateCloudStorageProvider(cloudStorageProviderEditorViewModel);
                            break;
                        }

                    case Enums.PageEditorMode.Edit:
                        {
                            // Apply any updates required here (if required)
                            break;
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
            var newRef = await _cloudStorageProviderConfigLoaderService.Add(config, CancellationToken.None);
            CloudStorageProviderConfigRefs.Add(newRef);
        }

        private async Task AddCloudStorageProviderCommandHandler()
        {
            var viewModel = new CloudStorageProviderEditorViewModel(this);
            var page = new Pages.CloudStorageProviderEditorPage(viewModel);
            await Application.Current.MainPage.Navigation.PushModalAsync(page, true);
        }

        private async Task RemoveSelectedCloudStorageProviderHandler()
        {
            await Task.Yield();

            if(SelectedCloudStorageProviderConfigRef == null)
            {
                return;
            }

            var selected = SelectedCloudStorageProviderConfigRef;
            CloudStorageProviderConfigRefs.Remove(selected);
            SelectedCloudStorageProviderConfigRef = null;
        }
    }
}
