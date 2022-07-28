using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Client.ViewModels.Enums;
using devoctomy.Passchamp.Core.Cloud.AmazonS3;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public partial class CloudStorageProviderEditorViewModel : BaseViewModel
    {
        public PageEditorMode EditorMode { get; }

        [ObservableProperty]
        private string displayName;

        [ObservableProperty]
        private string accessId;

        [ObservableProperty]
        private string secretKey;

        [ObservableProperty]
        private string region;

        [ObservableProperty]
        private string bucket;

        [ObservableProperty]
        private string path;

        private readonly string _id;

        public SettingsViewModel SettingsViewModel { get; set; }

        public IAsyncRelayCommand OkCommand { get; }

        public CloudStorageProviderEditorViewModel(SettingsViewModel settingsViewModel)
        {
            OkCommand = new AsyncRelayCommand(OkCommandHandler);

            SettingsViewModel = settingsViewModel;
        }

        public CloudStorageProviderEditorViewModel(
            AmazonS3CloudStorageProviderConfig amazonS3CloudStorageProviderConfig,
            SettingsViewModel settingsViewModel)
        {
            OkCommand = new AsyncRelayCommand(OkCommandHandler);

            _id = amazonS3CloudStorageProviderConfig.Id;
            DisplayName = amazonS3CloudStorageProviderConfig.DisplayName;
            AccessId = amazonS3CloudStorageProviderConfig.AccessId;
            SecretKey = amazonS3CloudStorageProviderConfig.SecretKey;
            Region = amazonS3CloudStorageProviderConfig.Region;
            Bucket = amazonS3CloudStorageProviderConfig.Bucket;
            Path = amazonS3CloudStorageProviderConfig.Path;

            SettingsViewModel = settingsViewModel;
        }

        private Task OkCommandHandler()
        {
            return SettingsViewModel.Return(this);
        }
    }
}
