using CommunityToolkit.Mvvm.ComponentModel;

namespace devoctomy.Passchamp.Client.ViewModels.Base;

public abstract class BaseAppShellViewModel : ObservableObject
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

