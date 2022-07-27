using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public partial class CloudStorageProviderEditorViewModel : BaseViewModel
    {
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

        public SettingsViewModel ReturnViewModel { get; set; }

        public IAsyncRelayCommand OkCommand { get; }

        public CloudStorageProviderEditorViewModel()
        {
            OkCommand = new AsyncRelayCommand(OkCommandHandler);
        }

        private Task OkCommandHandler()
        {
            return ReturnViewModel.Return(this);
        }
    }
}
