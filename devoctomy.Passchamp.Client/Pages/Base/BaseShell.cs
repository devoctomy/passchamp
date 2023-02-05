﻿using devoctomy.Passchamp.Client.ViewModels.Base;
using System.Diagnostics;

namespace devoctomy.Passchamp.Client.Pages.Base;

public abstract class BaseShell<TViewModel> : BaseShell where TViewModel : BaseAppShellViewModel
{
    public BaseShell(TViewModel viewModel) : base(viewModel)
    {
    }

    public new TViewModel BindingContext => (TViewModel)base.BindingContext;
}

public abstract class BaseShell : Shell
{
    private readonly BaseAppShellViewModel _viewModel;

    public BaseShell(object viewModel = null)
    {
        _viewModel = viewModel as BaseAppShellViewModel;
        BindingContext = viewModel;
        Padding = 12;

        SetDynamicResource(BackgroundColorProperty, "AppBackgroundColor");

        if (string.IsNullOrWhiteSpace(Title))
        {
            Title = GetType().Name;
        }
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
}