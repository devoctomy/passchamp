using devoctomy.Passchamp.Client.ViewModels.Base;

namespace devoctomy.Passchamp.Client.Views.Base;

public abstract class BaseView<TViewModel> : BaseView where TViewModel : BaseViewModel
{
    protected BaseView(TViewModel viewModel) : base(viewModel)
    {
    }

    public new TViewModel BindingContext => (TViewModel)base.BindingContext;
}

public abstract class BaseView : ContentView
{
    private readonly BaseViewModel _viewModel;

    protected BaseView()
    {
        SetDynamicResource(BackgroundColorProperty, "AppBackgroundColor");
    }

    protected BaseView(object viewModel)
    {
        _viewModel = viewModel as BaseViewModel;
        BindingContext = viewModel;

        SetDynamicResource(BackgroundColorProperty, "AppBackgroundColor");
    }
}
