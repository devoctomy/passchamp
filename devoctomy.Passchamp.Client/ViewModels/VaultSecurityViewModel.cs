using CommunityToolkit.Mvvm.ComponentModel;
using devoctomy.Passchamp.Client.ViewModels.Base;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class VaultSecurityViewModel : BaseViewModel
{
    [ObservableProperty]
    private string masterPassphrase;
}
