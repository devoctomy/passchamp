using System.Windows.Input;

namespace devoctomy.Passchamp.Client.Controls;

public partial class TabView : ContentView
{
    public event EventHandler<SelectedTabViewPageChangedEventArgs> SelectedTabViewPageChanged;

    public static BindableProperty TabHeightProperty = BindableProperty.Create(
        nameof(TabHeight),
        typeof(double),
        typeof(TabView),
        64d);

    public static BindableProperty TabPagesProperty = BindableProperty.Create(
        nameof(TabPages),
        typeof(IList<TabViewPage>),
        typeof(TabView),
        new List<TabViewPage>());

    public static BindableProperty TabAccentColourProperty = BindableProperty.Create(
        nameof(TabPages),
        typeof(Color),
        typeof(TabView),
        Colors.Red);

    public static BindableProperty SelectedTabAccentColourProperty = BindableProperty.Create(
        nameof(TabPages),
        typeof(Color),
        typeof(TabView),
        Colors.Blue);

    public double TabHeight
    {
        get
        {
            return (double)GetValue(TabHeightProperty);
        }
        set
        {
            SetValue(TabHeightProperty, value);
        }
    }

    public IList<TabViewPage> TabPages
    {
        get
        {
            return (IList<TabViewPage>)GetValue(TabPagesProperty);
        }
        set
        {
            SetValue(TabPagesProperty, value);
        }
    }

    public Color TabAccentColour
    {
        get
        {
            return (Color)GetValue(TabAccentColourProperty);
        }
        set
        {
            SetValue(TabAccentColourProperty, value);
        }
    }

    public Color SelectedTabAccentColour
    {
        get
        {
            return (Color)GetValue(SelectedTabAccentColourProperty);
        }
        set
        {
            SetValue(SelectedTabAccentColourProperty, value);
        }
    }

    public ICommand SelectionChangedCommand { get; }

    public TabView()
	{
        InitializeComponent();
	}

    public void SelectPage(TabViewPage tabViewPage)
    {
        TabPages.ToList().ForEach((TabViewPage page) =>
        {
            page.IsSelected = page == tabViewPage;
        });
        TabPageCollection.SelectedItem = tabViewPage;
    }

    private void UpdateContentView()
    {
        var selectedTabPage = TabPageCollection.SelectedItem as TabViewPage;
        if (selectedTabPage != null && selectedTabPage.ContentType != null)
        {
            // !!! HACK !!! This is a bit of a hack for now just to test. Need to pass binding along without any object access
            var binding = selectedTabPage.BindingContext as Binding;
            var content = (View)Activator.CreateInstance(selectedTabPage.ContentType, ((Page)binding.Source).BindingContext);
            TabContent.Content = content;
        }
        else
        {
            TabContent.Content = null;
        }
        SelectedTabViewPageChanged?.Invoke(
            this,
            new SelectedTabViewPageChangedEventArgs
            {
                SelectedTabViewPage = selectedTabPage
            });
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var previousTabViewPage = (TabViewPage)e.PreviousSelection.FirstOrDefault();
        if (previousTabViewPage != null)
        {
            previousTabViewPage.IsSelected = false;
        }

        var currentTabViewPage = (TabViewPage)e.CurrentSelection.FirstOrDefault();
        if (currentTabViewPage != null)
        {
            currentTabViewPage.IsSelected = true;
        }

        UpdateContentView();
    }
}