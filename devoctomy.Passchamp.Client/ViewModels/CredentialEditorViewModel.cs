using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Cloud;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class CredentialEditorViewModel : BaseViewModel
{
    public BaseViewModel ReturnViewModel { get; }

    public CredentialEditorViewModel(BaseViewModel returnViewModel)
    {
        ReturnViewModel = returnViewModel;
    }

    [RelayCommand]
    private async Task Back()
    {
        await ReturnViewModel.Return(null);
    }

    [RelayCommand]
    private async Task Ok()
    {
        await ReturnViewModel.Return(null);
    }
}
