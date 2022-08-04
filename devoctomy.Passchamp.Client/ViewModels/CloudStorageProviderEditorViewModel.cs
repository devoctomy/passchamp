using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Client.ViewModels.Enums;
using devoctomy.Passchamp.Core.Cloud.AmazonS3;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public partial class CloudStorageProviderEditorViewModel : BaseViewModel
    {
        public string Id { get; set; }

        [ObservableProperty]
        private PageEditorMode editorMode;

        [ObservableProperty]
        private string displayName;

        [ObservableProperty]
        private string accessId;

        [ObservableProperty]
        private string secretKey;

        [ObservableProperty]
        private string region = Amazon.RegionEndpoint.EUWest2.DisplayName;

        [ObservableProperty]
        private string bucket;

        [ObservableProperty]
        private string path;

        [ObservableProperty]
        public List<string> regions = Amazon.RegionEndpoint.EnumerableAllRegions.Select(x => x.DisplayName).ToList();

        public BaseViewModel ReturnViewModel { get; set; }

        public IAsyncRelayCommand BackCommand { get; set; }
        public IAsyncRelayCommand OkCommand { get; }

        public CloudStorageProviderEditorViewModel(BaseViewModel returnViewModel)
        {
            OkCommand = new AsyncRelayCommand(OkCommandHandler);

            EditorMode = PageEditorMode.Create;
            ReturnViewModel = returnViewModel;
        }

        public CloudStorageProviderEditorViewModel(
            AmazonS3CloudStorageProviderConfig amazonS3CloudStorageProviderConfig,
            BaseViewModel returnViewModel)
        {
            BackCommand = new AsyncRelayCommand(BackCommandHandler);
            OkCommand = new AsyncRelayCommand(OkCommandHandler);

            EditorMode = PageEditorMode.Edit;
            Id = amazonS3CloudStorageProviderConfig.Id;
            DisplayName = amazonS3CloudStorageProviderConfig.DisplayName;
            AccessId = amazonS3CloudStorageProviderConfig.AccessId;
            SecretKey = amazonS3CloudStorageProviderConfig.SecretKey;
            Region = amazonS3CloudStorageProviderConfig.Region;
            Bucket = amazonS3CloudStorageProviderConfig.Bucket;
            Path = amazonS3CloudStorageProviderConfig.Path;

            ReturnViewModel = returnViewModel;
        }

        public async Task BackCommandHandler()
        {
            await ReturnViewModel.Return(null);
        }

        private async Task OkCommandHandler()
        {
            await ReturnViewModel.Return(this);
        }
    }
}
