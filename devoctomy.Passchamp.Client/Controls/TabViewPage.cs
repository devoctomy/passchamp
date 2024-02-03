using CommunityToolkit.Mvvm.ComponentModel;

namespace devoctomy.Passchamp.Client.Controls;

public partial class TabViewPage : ObservableObject
{
    [ObservableProperty]
    private bool isSelected;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private double width;

    [ObservableProperty]
    private ContentView content;

    [ObservableProperty]
    private Type contentType;

    [ObservableProperty]
    private string viewModelPropertyName;
}
