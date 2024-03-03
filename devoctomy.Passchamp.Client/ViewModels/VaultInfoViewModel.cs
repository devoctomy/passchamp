using CommunityToolkit.Mvvm.ComponentModel;
using devoctomy.Passchamp.Client.ViewModels.Base;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class VaultInfoViewModel : BaseViewModel
{
    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string description;
}
