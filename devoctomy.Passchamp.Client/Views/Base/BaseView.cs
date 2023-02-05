using devoctomy.Passchamp.Client.ViewModels.Base;
using System.Diagnostics;

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

    public BaseView(object viewModel = null)
    {
        _viewModel = viewModel as BaseViewModel;
        BindingContext = viewModel;

        SetDynamicResource(BackgroundColorProperty, "AppBackgroundColor");
    }
}
