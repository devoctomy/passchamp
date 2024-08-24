using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Maui.IO;
using System.Diagnostics;

namespace devoctomy.Passchamp.Client.Pages.Base;

public abstract class BaseShell<TViewModel> : BaseShell where TViewModel : BaseAppShellViewModel
{
    protected BaseShell(TViewModel viewModel) : base(viewModel)
    {
    }

    public new TViewModel BindingContext => (TViewModel)base.BindingContext;
}

public abstract class BaseShell : Shell
{
    private readonly BaseAppShellViewModel _viewModel;

    protected BaseShell()
    {
        Padding = 12;

        SetDynamicResource(BackgroundColorProperty, "AppBackgroundColor");

        if (string.IsNullOrWhiteSpace(Title))
        {
            Title = GetType().Name;
        }
    }

    protected BaseShell(object viewModel)
        : this()
    {
        _viewModel = viewModel as BaseAppShellViewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Debug.WriteLine($"OnAppearing: {Title}");
            
        Task.Run(_viewModel.OnAppearingAsync);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        Debug.WriteLine($"OnDisappearing: {Title}");
    }

    protected override void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);
        _viewModel.Navigating(args);
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        _viewModel.Navigated(args);
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
#if ANDROID
        var pathResolver = (Maui.Pathforms.Android.IO.PathResolver)MauiProgram.MauiApp.Services.GetService<IPathResolverService>();
        pathResolver.Initialise(this.Handler.MauiContext);
#endif
    }
}