using devoctomy.Passchamp.Client.ViewModels.Base;
using System.Diagnostics;

namespace devoctomy.Passchamp.Client.Pages.Base;

public abstract class BasePage<TViewModel> : BasePage where TViewModel : BaseViewModel
{
    protected BasePage(TViewModel viewModel) : base(viewModel)
    {
    }

    public new TViewModel BindingContext => (TViewModel)base.BindingContext;
}

public abstract class BasePage : ContentPage
{
    public bool TransientViewModel { get; set; }

    public BaseViewModel ViewModel { get; private set; }

    public BasePage()
    {
        Padding = 12;

        SetDynamicResource(BackgroundColorProperty, "AppBackgroundColor");

        if (string.IsNullOrWhiteSpace(Title))
        {
            Title = GetType().Name;
        }
    }

    public BasePage(object viewModel)
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
