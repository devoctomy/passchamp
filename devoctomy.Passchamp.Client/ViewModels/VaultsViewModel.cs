﻿using CommunityToolkit.Mvvm.ComponentModel;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Vault;
using System.Collections.ObjectModel;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public partial class VaultsViewModel : BaseViewModel
    {
        public ObservableCollection<Vault> Vaults { get; } = new ObservableCollection<Vault>();

        [ObservableProperty]
        private Vault itemSelected = null;

        private readonly ICloudStorageProviderConfigLoaderService _cloudStorageProviderConfigLoaderService;

        public VaultsViewModel(ICloudStorageProviderConfigLoaderService cloudStorageProviderConfigLoaderService)
        {
            _cloudStorageProviderConfigLoaderService = cloudStorageProviderConfigLoaderService;
            Vaults.Add(new Vault
            {
                Header = new VaultHeader
                {
                    FormatVersion = "1.0"
                }
            });
        }
    }
}
