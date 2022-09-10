namespace devoctomy.Passchamp.Client.ViewModels.Base;

public abstract partial class BaseAppShellPageViewModel : BaseViewModel
{
    public List<MenuItem> MenuItems { get; set; }

    private readonly static SemaphoreSlim _loaderLock = new(1, 1);
    private bool _loaded = false;

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

            var appShellViewModel = Shell.Current.BindingContext as AppShellViewModel;
            await appShellViewModel.OnCurrentPageChangeAsync();
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

    protected void SetupMenuItems()
    {
        MenuItems = new List<MenuItem>();
        OnSetupMenuItems();
    }

    public abstract Task OnFirstAppearanceAsync(); 
    public abstract void OnSetupMenuItems();
}