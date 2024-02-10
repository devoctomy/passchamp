using CommunityToolkit.Mvvm.ComponentModel;
using devoctomy.Passchamp.Client.ViewModels.Base;
using System.Collections.ObjectModel;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class VaultSecurityViewModel : BaseViewModel
{
    [ObservableProperty]
    private string masterPassphrase;

    [ObservableProperty]
    private string graph;

    [ObservableProperty]
    private ObservableCollection<string> graphs;

    public VaultSecurityViewModel()
    {
    }
}
