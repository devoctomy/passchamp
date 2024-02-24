using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Graph.Presets;
using System.Collections.ObjectModel;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class VaultSecurityViewModel : BaseViewModel
{
    [ObservableProperty]
    private string masterPassphrase;

    [ObservableProperty]
    private string graph;

    [ObservableProperty]
    private ObservableCollection<IGraphPresetSet> graphPresetSets;

    [ObservableProperty]
    private IGraphPresetSet selectedGraphPresetSet;

    public VaultSecurityViewModel(IEnumerable<IGraphPresetSet> graphPresetSets)
    {
        GraphPresetSets = graphPresetSets.ToObservableCollection();
        SelectedGraphPresetSet = GraphPresetSets.SingleOrDefault(x => x.Default);
    }
}
