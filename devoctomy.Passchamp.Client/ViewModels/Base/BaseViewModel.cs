using CommunityToolkit.Mvvm.ComponentModel;

namespace devoctomy.Passchamp.Client.ViewModels.Base;

public abstract partial class BaseViewModel : ObservableObject
{
    public virtual Task OnAppearingAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task Return(BaseViewModel fromViewModel)
    {
        throw new NotImplementedException();
    }
}

