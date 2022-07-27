using CommunityToolkit.Mvvm.ComponentModel;
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
    }
}
