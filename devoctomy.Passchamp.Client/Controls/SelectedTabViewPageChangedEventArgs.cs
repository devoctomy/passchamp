using devoctomy.Passchamp.Client.ViewModels.Base;

namespace devoctomy.Passchamp.Client.Controls;

public class SelectedTabViewPageChangedEventArgs : EventArgs
{
    public TabViewPage SelectedTabViewPage { get; set; }
    public BaseViewModel ViewModel { get; set; }
}

