using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;

namespace devoctomy.Passchamp.Client.ViewModels.Base;

public abstract class BaseViewModel : ObservableObject
{
    public virtual Task Init()
    {
        return Task.CompletedTask;
    }

    public virtual Task OnAppearingAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task Return(BaseViewModel fromViewModel)
    {
        throw new NotImplementedException();
    }
}

