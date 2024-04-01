using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Core.Vault;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class CredentialEditorViewModel : BaseViewModel
{
    public BaseViewModel ReturnViewModel { get; }

    [ObservableProperty]
    private Credential credential;

    [ObservableProperty]
    private string confirmPassword;

    public CredentialEditorViewModel(BaseViewModel returnViewModel)
    {
        ReturnViewModel = returnViewModel;
    }

    public override Task OnAppearingAsync()
    {
        ConfirmPassword = Credential?.Password;
        return Task.CompletedTask;
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
