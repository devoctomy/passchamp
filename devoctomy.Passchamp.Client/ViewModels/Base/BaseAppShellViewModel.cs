using CommunityToolkit.Mvvm.ComponentModel;

namespace devoctomy.Passchamp.Client.ViewModels.Base;

[INotifyPropertyChanged]
public abstract partial class BaseAppShellViewModel
{
    public virtual Task OnAppearingAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task Return(BaseViewModel fromViewModel)
    {
        throw new NotImplementedException();
    }

    public virtual void Navigating(ShellNavigatingEventArgs args)
    {
    }

    public virtual void Navigated(ShellNavigatedEventArgs args)
    {
    }
}

