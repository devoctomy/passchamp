using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Maui.Models;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class EnterMasterPassphraseViewModel : BaseViewModel
{
    [ObservableProperty]
    private string masterPassphrase;

    [ObservableProperty]
    private VaultIndex vaultIndex;

    public BaseViewModel ReturnViewModel { get; }

    public override string InitialControlNameFocus => "MasterPassphraseEntry";


    public EnterMasterPassphraseViewModel(BaseViewModel returnViewModel)
    {
        ReturnViewModel = returnViewModel;
    }

    [RelayCommand]
    private async Task Ok(object param)
    {
        await ReturnViewModel.Return(this);
    }

    [RelayCommand]
    private async Task Back(object param)
    {
        await ReturnViewModel.Return(null);
    }

    public override Task OnAppearingAsync()
    {
        return base.OnAppearingAsync();
    }
}
