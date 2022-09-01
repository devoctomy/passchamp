using devoctomy.Passchamp.Client.ViewModels.Base;
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

    public static BindableProperty SelectedTabChangedCommandProperty = BindableProperty.Create(
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

    public BaseViewModel GetTabViewPageViewModel(TabViewPage tabViewPage)
    {
        if(tabViewPage == null)
        {
            return null;
        }

        var page = Parent;
        while (page.GetType().IsAssignableFrom(typeof(Page)))
        {
            page = page.Parent;
        }
        var context = page.BindingContext;
        var propInfo = context.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList().SingleOrDefault(x => x.Name == tabViewPage.ViewModelPropertyName);
        var viewModel = propInfo.GetValue(context) as BaseViewModel;
        return viewModel;
    }

    private void UpdateContentView()
    {
        var selectedTabPage = TabPageCollection.SelectedItem as TabViewPage;
        var viewModel = GetTabViewPageViewModel(selectedTabPage);
        if (selectedTabPage != null && selectedTabPage.ContentType != null)
        {
            // !!! HACK !!! This is a bit of a hack for now just to test. Need to pass binding along without any object access
            // this is currently being done like this due to [ObservaleProperty] attribute not being compatible with BindableObject
            // and i'm not entirely sure how to make them work.           
            var content = (View)Activator.CreateInstance(selectedTabPage.ContentType, viewModel);
            TabContent.Content = content;
        }
        else
        {
            TabContent.Content = null;
        }
        var args = new SelectedTabViewPageChangedEventArgs
        {
            SelectedTabViewPage = selectedTabPage,
            ViewModel = viewModel
        };
        SelectedTabViewPageChanged?.Invoke(
            this,
            args);
        SelectedTabChangedCommand?.Execute(args);
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

    private void TabPageCollection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        // !!! HACK !!! This gets the initial selected tab working in Windows.  For some reason
        // this works fine in Android but not in Windows so we have to do this hack.
        if (e.PropertyName == nameof(TabPageCollection.SelectedItem)
            && TabPageCollection.SelectedItem == null)
        {
            SelectSingleIsSelectedPage();
        }
    }
}