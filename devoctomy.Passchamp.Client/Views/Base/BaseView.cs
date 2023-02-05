using devoctomy.Passchamp.Client.ViewModels.Base;

namespace devoctomy.Passchamp.Client.Views.Base;

public abstract class BaseView<TViewModel> : BaseView where TViewModel : BaseViewModel
{
    public BaseView(TViewModel viewModel) : base(viewModel)
    {
    }

    public new TViewModel BindingContext => (TViewModel)base.BindingContext;
}

public abstract class BaseView : ContentView
{
    private readonly BaseViewModel _viewModel;

    public BaseView()
    {
        SetDynamicResource(BackgroundColorProperty, "AppBackgroundColor");
    }

    public BaseView(object viewModel)
    {
        _viewModel = viewModel as BaseViewModel;
        BindingContext = viewModel;

        SetDynamicResource(BackgroundColorProperty, "AppBackgroundColor");
    }
}
