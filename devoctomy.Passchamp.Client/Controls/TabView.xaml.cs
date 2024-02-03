using devoctomy.Passchamp.Client.ViewModels.Base;
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

    public readonly BindableProperty TabPagesProperty = BindableProperty.Create(
        nameof(TabPages),
        typeof(IList<TabViewPage>),
        typeof(TabView),
        new List<TabViewPage>());

    public static readonly BindableProperty TabBackColourProperty = BindableProperty.Create(
        nameof(TabBackColour),
        typeof(Color),
        typeof(TabView),
        Colors.Transparent);

    public static readonly BindableProperty SelectedTabBackColourProperty = BindableProperty.Create(
        nameof(SelectedTabBackColour),
        typeof(Color),
        typeof(TabView),
        Colors.Transparent);

    public static readonly BindableProperty TextColourProperty = BindableProperty.Create(
        nameof(TextColour),
        typeof(Color),
        typeof(TabView),
        Colors.Black);

    public static readonly BindableProperty TabAccentColourProperty = BindableProperty.Create(
        nameof(TabAccentColour),
        typeof(Color),
        typeof(TabView),
        Colors.Transparent);

    public static readonly BindableProperty SelectedTabAccentColourProperty = BindableProperty.Create(
        nameof(SelectedTabAccentColour),
        typeof(Color),
        typeof(TabView),
        Colors.White);

    public static readonly BindableProperty SelectedTabChangedCommandProperty = BindableProperty.Create(
        nameof(SelectedTabChangedCommand),
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

    public Color TabBackColour
    {
        get
        {
            return (Color)GetValue(TabBackColourProperty);
        }
        set
        {
            SetValue(TabBackColourProperty, value);
        }
    }

    public Color SelectedTabBackColour
    {
        get
        {
            return (Color)GetValue(SelectedTabBackColourProperty);
        }
        set
        {
            SetValue(SelectedTabBackColourProperty, value);
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

    public Color TextColour
    {
        get
        {
            return (Color)GetValue(TextColourProperty);
        }
        set
        {
            SetValue(TextColourProperty, value);
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

    private TabViewPage _selectedPage;

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
        if(tabViewPage == null || string.IsNullOrEmpty(tabViewPage.ViewModelPropertyName))
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
            TabContent.Content = selectedTabPage?.Content;
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
        var currentTabViewPage = (TabViewPage)e.CurrentSelection.FirstOrDefault();
        if(currentTabViewPage == null)
        {
            return;
        }

        if (currentTabViewPage != null)
        {
            currentTabViewPage.IsSelected = true;
        }

        var previousTabViewPage = (TabViewPage)e.PreviousSelection.FirstOrDefault();
        if (previousTabViewPage != null)
        {
            previousTabViewPage.IsSelected = false;
        }

        _selectedPage = currentTabViewPage;

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