using System.Windows.Input;

namespace devoctomy.Passchamp.Client.Controls;

public partial class TabView : ContentView
{
    public event EventHandler<SelectedTabViewPageChangedEventArgs> SelectedTabViewPageChanged;

    public static readonly BindableProperty TabHeightProperty = BindableProperty.Create(
        nameof(TabHeight),
        typeof(double),
        typeof(TabView),
        64d);

    public static readonly BindableProperty TabPagesProperty = BindableProperty.Create(
        nameof(TabPages),
        typeof(IList<TabViewPage>),
        typeof(TabView),
        new List<TabViewPage>());

    public static readonly BindableProperty TabAccentColourProperty = BindableProperty.Create(
        nameof(TabPages),
        typeof(Color),
        typeof(TabView),
        Colors.Red);

    public static readonly BindableProperty SelectedTabAccentColourProperty = BindableProperty.Create(
        nameof(TabPages),
        typeof(Color),
        typeof(TabView),
        Colors.Blue);

    public static readonly BindableProperty SelectedTabChangedCommandProperty = BindableProperty.Create(
        nameof(TabPages),
        typeof(ICommand),
        typeof(TabView),
        null);

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

    public ICommand SelectedTabChangedCommand
    {
        get
        {
            return (ICommand)GetValue(SelectedTabChangedCommandProperty);
        }
        set
        {
            SetValue(SelectedTabChangedCommandProperty, value);
        }
    }

    public ICommand SelectionChangedCommand { get; }

    public TabView()
	{
        InitializeComponent();
    }

    protected override void OnParentChanged()
    {
        base.OnParentChanged();
        SelectSingleIsSelectedPage();    // This immediately gets set back to null
    }

    public void SelectSingleIsSelectedPage()
    {
        var selected = TabPages.SingleOrDefault(x => x.IsSelected);
        if (selected != null)
        {
            SelectPage(selected);
        }
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
            // this is currently being done like this due to [ObservaleProperty] attribute not being compatible with BindableObject
            // and i'm not entirely sure how to make them work.
            var page = Parent;
            while(page.GetType().IsAssignableFrom(typeof(Page)))
            {
                page = page.Parent;
            }
            var context = page.BindingContext;
            var propInfo = context.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList().SingleOrDefault(x => x.Name == selectedTabPage.ViewModelPropertyName);
            var viewModel = propInfo.GetValue(context);
            var content = (View)Activator.CreateInstance(selectedTabPage.ContentType, viewModel);
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
        SelectedTabChangedCommand?.Execute(selectedTabPage);
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