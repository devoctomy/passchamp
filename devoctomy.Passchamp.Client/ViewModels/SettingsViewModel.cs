using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Cloud.AmazonS3;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public partial class SettingsViewModel : BaseAppShellPageViewModel
    {
        [ObservableProperty]
        ObservableCollection<CloudStorageProviderConfigRef> cloudStorageProviderConfigRefs;

        [ObservableProperty]
        CloudStorageProviderConfigRef selectedCloudStorageProviderConfigRef;

        public ICommand AcceptCommand { get; }
        public ICommand CancelCommand { get; }
        public IAsyncRelayCommand AddCloudStorageProviderCommand { get; }
        public IAsyncRelayCommand EditSelectedCloudStorageProviderCommand { get; }
        public IAsyncRelayCommand RemoveSelectedCloudStorageProviderCommand { get; }

        [ObservableProperty]
        private bool removeSelectedCloudStorageProviderCommandCanExecute;

        private readonly ICloudStorageProviderConfigLoaderService _cloudStorageProviderConfigLoaderService;
        private readonly static SemaphoreSlim _loaderLock = new(1, 1);
        private bool _loaded = false;

        public SettingsViewModel(ICloudStorageProviderConfigLoaderService cloudStorageProviderConfigLoaderService)
        {
            AcceptCommand = new Command(AcceptCommandHandler);
            CancelCommand = new Command(CancelCommandHandler);
            AddCloudStorageProviderCommand = new AsyncRelayCommand(AddCloudStorageProviderCommandHandler);
            EditSelectedCloudStorageProviderCommand = new AsyncRelayCommand(EditCloudStorageProviderCommandHandler);
            RemoveSelectedCloudStorageProviderCommand = new AsyncRelayCommand(RemoveSelectedCloudStorageProviderHandler);
            removeSelectedCloudStorageProviderCommandCanExecute = false;
            _cloudStorageProviderConfigLoaderService = cloudStorageProviderConfigLoaderService;
            SetupMenuItems();
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

                var appShellViewModel = Shell.Current.BindingContext as AppShellViewModel;
                await appShellViewModel.OnCurrentPageChangeAsync();
            }
            finally
            {
                _loaderLock.Release();
            }  
        }

        public override async Task Return(BaseViewModel viewModel)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();

            if(viewModel == null)
            {
                return;
            }

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
                            await UpdateCloudStorageProvider(cloudStorageProviderEditorViewModel);
                            break;
                        }
                }
            }
        }

        public override void OnSetupMenuItems()
        {
            MenuItems.Add(new MenuItem
            {
                Text = "Accept",
                Command = AcceptCommand
            });
            MenuItems.Add(new MenuItem
            {
                Text = "Cancel",
                Command = CancelCommand
            });
        }

        private void CancelCommandHandler(object param)
        {
            Shell.Current.GoToAsync("//Vaults");    // Need to go back to previous here ".." does not work
        }

        private void AcceptCommandHandler(object param)
        {
            Shell.Current.GoToAsync("//Vaults");    // Need to go back to previous here ".." does not work
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
            if(SelectedCloudStorageProviderConfigRef == null)
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
}
