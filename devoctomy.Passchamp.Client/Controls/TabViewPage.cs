using CommunityToolkit.Mvvm.ComponentModel;

namespace devoctomy.Passchamp.Client.Controls;

public partial class TabViewPage : ObservableObject
{
    [ObservableProperty]
    private bool isSelected;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private double width = 60d;

    [ObservableProperty]
    private Type contentType;
}

