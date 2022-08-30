using System.Collections.ObjectModel;

namespace devoctomy.Passchamp.Client.ViewModels.Base;

public abstract partial class BaseAppShellPageViewModel : BaseViewModel
{
    public List<MenuItem> MenuItems { get; set; }

    public BaseAppShellPageViewModel()
    {
    }

    public override Task OnAppearingAsync()
    {
        return Task.CompletedTask;
    }

    public override Task Return(BaseViewModel fromViewModel)
    {
        throw new NotImplementedException();
    }

    protected void SetupMenuItems()
    {
        MenuItems = new List<MenuItem>();
        OnSetupMenuItems();
    }

    public abstract void OnSetupMenuItems();
}