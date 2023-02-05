namespace devoctomy.Passchamp.Client.ViewModels.Base;

public abstract partial class BaseAppShellPageViewModel : BaseViewModel
{
    public List<MenuItem> MenuItems { get; set; }

    private readonly static SemaphoreSlim _loaderLock = new(1, 1);
    private bool _loaded = false;
    private string _previousTheme = App.Current.UserAppTheme.ToString();

    public BaseAppShellPageViewModel()
    {
    }

    public override async Task OnAppearingAsync()
    {
        await _loaderLock.WaitAsync();
        try
        {
            if (!_loaded)
            {
                await OnFirstAppearanceAsync();
                _loaded = true;
            }

            var currentAppTheme = App.Current.UserAppTheme.ToString();
            if(currentAppTheme != _previousTheme)
            {
                SetupMenuItems();
            }

            var appShellViewModel = Shell.Current.BindingContext as AppShellViewModel;
            await appShellViewModel.OnCurrentPageChangeAsync();
            _previousTheme = App.Current.UserAppTheme.ToString();
        }
        finally
        {
            _loaderLock.Release();
        }
    }

    public override Task Return(BaseViewModel fromViewModel)
    {
        throw new NotImplementedException();
    }

    public void SetupMenuItems()
    {
        MenuItems = new List<MenuItem>();
        OnSetupMenuItems();
    }

    protected async Task RefreshMenuItems()
    {
        var appShell = Shell.Current as AppShellPage;
        var appShellViewModel = appShell.BindingContext;
        await appShellViewModel.RefreshMenuItems();
    }

    protected abstract Task OnFirstAppearanceAsync(); 
    protected abstract void OnSetupMenuItems();
}