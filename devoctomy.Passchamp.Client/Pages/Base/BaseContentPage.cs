using devoctomy.Passchamp.Client.ViewModels.Base;
using System.Diagnostics;

namespace devoctomy.Passchamp.Client.Pages.Base;

public abstract class BaseContentPage<TViewModel> : BaseContentPage where TViewModel : BaseViewModel
{
    protected BaseContentPage(TViewModel viewModel) : base(viewModel)
    {
        viewModel.Page = this;
    }

    public new TViewModel BindingContext => (TViewModel)base.BindingContext;
}

public abstract class BaseContentPage : ContentPage, IPage
{
    public bool TransientViewModel { get; set; }

    public BaseViewModel ViewModel { get; private set; }

    private bool _attachedFocusHandler = false;
    private bool _initialFocus = false;

    protected BaseContentPage()
    {
        Padding = 12;

        SetDynamicResource(BackgroundColorProperty, "AppBackgroundColor");

        if (string.IsNullOrWhiteSpace(Title))
        {
            Title = GetType().Name;
        }
    }

    protected BaseContentPage(object viewModel)
        : this()
    {
        ViewModel = viewModel as BaseViewModel;
        BindingContext = viewModel;
    }

    private void Page_Focused(object sender, FocusEventArgs e)
    {
        if (!_initialFocus)
        {
            _initialFocus = true;
            if(!string.IsNullOrEmpty(ViewModel.InitialControlNameFocus))
            {
                var input = ViewModel.Page.FindByName(ViewModel.InitialControlNameFocus) as InputView;
                input.Focus();
            }
        }
    }

    public void ResetViewModel()
    {
        var viewModelType = ViewModel.GetType();
        var newViewModel = MauiProgram.MauiApp.Services.GetService(viewModelType) as BaseViewModel;
        ViewModel = newViewModel;
        BindingContext = newViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Debug.WriteLine($"OnAppearing: {Title}");

        if(!_attachedFocusHandler)
        {
            _attachedFocusHandler = true;
            ViewModel.Page.Focused += Page_Focused;
        }

        Task.Run(ViewModel.OnAppearingAsync);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        Debug.WriteLine($"OnDisappearing: {Title}");
    }
}
