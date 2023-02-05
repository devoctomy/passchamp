using devoctomy.Passchamp.Client.ViewModels.Base;

namespace devoctomy.Passchamp.Client.Controls;

public partial class TabViewPage : BindableObject
{
    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected),
        typeof(bool),
        typeof(TabViewPage),
        false);

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(TabViewPage),
        string.Empty);

    public static readonly BindableProperty WidthProperty = BindableProperty.Create(
        nameof(Width),
        typeof(double),
        typeof(TabViewPage),
        60d);

    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content),
        typeof(ContentView),
        typeof(TabViewPage),
        null);

    public static readonly BindableProperty ContentTypeProperty = BindableProperty.Create(
        nameof(ContentType),
        typeof(Type),
        typeof(TabViewPage),
        null);

    public static readonly BindableProperty ViewModelPropertyNameProperty = BindableProperty.Create(
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

    public ContentView Content
    {
        get => (ContentView)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
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
