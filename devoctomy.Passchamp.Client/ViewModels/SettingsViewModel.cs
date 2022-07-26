﻿using CommunityToolkit.Mvvm.ComponentModel;
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
            AddCloudStorageProviderCommand = new AsyncRelayCommand(AddCloudStorageProvider);
            RemoveSelectedCloudStorageProviderCommand = new AsyncRelayCommand(RemoveSelectedCloudStorageProvider);
            removeSelectedCloudStorageProviderCommandCanExecute = false;
        }

        private async Task AddCloudStorageProvider()
        {
            await Task.Yield();
            CloudStorageProviderConfigRefs.Add(
                new CloudStorageProviderConfigRef
                {
                    Id = "Another Test",
                    ProviderServiceTypeId = CloudStorageProviderServiceAttributeUtility.Get(typeof(AmazonS3CloudStorageProviderService)).TypeId
                });
        }

        private async Task RemoveSelectedCloudStorageProvider()
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