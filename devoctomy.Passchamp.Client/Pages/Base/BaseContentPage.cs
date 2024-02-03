using devoctomy.Passchamp.Client.ViewModels.Base;
using System.Diagnostics;

namespace devoctomy.Passchamp.Client.Pages.Base;

public abstract class BaseContentPage<TViewModel> : BaseContentPage where TViewModel : BaseViewModel
{
    protected BaseContentPage(TViewModel viewModel) : base(viewModel)
    {
    }

    public new TViewModel BindingContext => (TViewModel)base.BindingContext;
}

public abstract class BaseContentPage : ContentPage, IPage
{
    public bool TransientViewModel { get; set; }

    public BaseViewModel ViewModel { get; private set; }

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

        Task.Run(ViewModel.OnAppearingAsync);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        Debug.WriteLine($"OnDisappearing: {Title}");
    }
}
