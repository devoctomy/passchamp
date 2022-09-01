using devoctomy.Passchamp.Client.ViewModels.Base;

namespace devoctomy.Passchamp.Client.Controls;

public partial class TabViewPage : BindableObject
{
    public static BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected),
        typeof(bool),
        typeof(TabViewPage),
        false);

    public static BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(TabViewPage),
        string.Empty);

    public static BindableProperty WidthProperty = BindableProperty.Create(
        nameof(Width),
        typeof(double),
        typeof(TabViewPage),
        60d);

    public static BindableProperty ContentTypeProperty = BindableProperty.Create(
        nameof(ContentType),
        typeof(Type),
        typeof(TabViewPage),
        null);

    public static BindableProperty ViewModelPropertyNameProperty = BindableProperty.Create(
        nameof(ViewModelPropertyName),
        typeof(string),
        typeof(TabViewPage),
        string.Empty);

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public double Width
    {
        get => (double)GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }
    public Type ContentType
    {
        get => (Type)GetValue(ContentTypeProperty);
        set => SetValue(ContentTypeProperty, value);
    }

    public string ViewModelPropertyName
    {
        get => (string)GetValue(ViewModelPropertyNameProperty);
        set => SetValue(ViewModelPropertyNameProperty, value);
    }
}
