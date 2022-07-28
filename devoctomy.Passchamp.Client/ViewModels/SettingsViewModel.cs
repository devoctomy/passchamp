using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Cloud.AmazonS3;
using devoctomy.Passchamp.Core.Cloud.Utility;
using System.Collections.ObjectModel;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        public ObservableCollection<CloudStorageProviderConfigRef> CloudStorageProviderConfigRefs { get; set; } = new ObservableCollection<CloudStorageProviderConfigRef>();

        [ObservableProperty]
        CloudStorageProviderConfigRef selectedCloudStorageProviderConfigRef;

        public IAsyncRelayCommand AddCloudStorageProviderCommand { get; }
        public IAsyncRelayCommand RemoveSelectedCloudStorageProviderCommand { get; }

        [ObservableProperty]
        private bool removeSelectedCloudStorageProviderCommandCanExecute;

        public SettingsViewModel()
        {
            AddCloudStorageProviderCommand = new AsyncRelayCommand(AddCloudStorageProviderCommandHandler);
            RemoveSelectedCloudStorageProviderCommand = new AsyncRelayCommand(RemoveSelectedCloudStorageProviderHandler);
            removeSelectedCloudStorageProviderCommandCanExecute = false;
        }

        public async Task Return(BaseViewModel viewModel)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();

            if (viewModel is CloudStorageProviderEditorViewModel)
            {
                var cloudStorageProviderEditorViewModel = viewModel as CloudStorageProviderEditorViewModel;
                switch(cloudStorageProviderEditorViewModel.EditorMode)
                {
                    case Enums.PageEditorMode.Create:
                        {
                            var providerRef = new CloudStorageProviderConfigRef
                            {
                                Id = cloudStorageProviderEditorViewModel.DisplayName,
                                ProviderServiceTypeId = CloudStorageProviderServiceAttributeUtility.Get(typeof(AmazonS3CloudStorageProviderService)).TypeId
                            };
                            CloudStorageProviderConfigRefs.Add(providerRef);
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
