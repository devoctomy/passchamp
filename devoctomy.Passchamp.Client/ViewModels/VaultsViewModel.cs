using CommunityToolkit.Mvvm.ComponentModel;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Vault;
using System.Collections.ObjectModel;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public partial class VaultsViewModel : BaseViewModel
    {
        [ObservableProperty]
        Vault itemSelected = null;

        public ObservableCollection<Vault> Vaults { get; } = new ObservableCollection<Vault>();

        public VaultsViewModel()
        {
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
