using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Cloud;
using System.Collections.ObjectModel;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        public ObservableCollection<ICloudStorageProviderConfig> CloudStorageProviderConfigs { get; set; }

        public IAsyncRelayCommand AddCloudStorageProviderCommand { get; }
        public IAsyncRelayCommand RemoveSelectedCloudStorageProviderCommand { get; }

        public SettingsViewModel()
        {
            AddCloudStorageProviderCommand = new AsyncRelayCommand(AddCloudStorageProvider);
            RemoveSelectedCloudStorageProviderCommand = new AsyncRelayCommand(RemoveSelectedCloudStorageProvider);
        }

        public void Load()
        {
            // Load the settings here
        }

        private async Task AddCloudStorageProvider()
        {
            await Task.Yield();
        }

        private async Task RemoveSelectedCloudStorageProvider()
        {
            await Task.Yield();
        }
    }
}
