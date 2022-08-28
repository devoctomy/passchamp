﻿using CommunityToolkit.Mvvm.ComponentModel;
using devoctomy.Passchamp.Client.ViewModels.Base;
using System.Windows.Input;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class AppShellViewModel : BaseViewModel
{
    [ObservableProperty]
    public ICommand homeCommand;

    [ObservableProperty]
    public ICommand menuSelectionChangedCommand;

    [ObservableProperty]
    public ICommand menuSelectionPropertyChangedCommand;

    private BaseAppShellPageViewModel _currentPageViewModel;

    public AppShellViewModel()
    {
        HomeCommand = new Command(HomeCommandhandler);
        menuSelectionChangedCommand = new Command(MenuSelectionChangedCommandHandler);
        menuSelectionPropertyChangedCommand = new Command(MenuSelectionPropertyChangedCommandHandler);
    }

    public async Task OnCurrentPageChangeAsync()
    {
        await Task.Yield();

        var page = Shell.Current.CurrentPage;
        var nextPageViewModel = page.BindingContext as BaseAppShellPageViewModel;
        if (_currentPageViewModel != nextPageViewModel)
        {
            await RemoveMenuItems();
            _currentPageViewModel = nextPageViewModel;
            await AddMenuItems();
        }
    }

    private async Task RemoveMenuItems()
    {
        if (_currentPageViewModel == null || _currentPageViewModel.MenuItems == null)
        {
            return;
        }

        var page = Shell.Current.CurrentPage;
        var appShell = Shell.Current as AppShell;
        await page.Dispatcher.DispatchAsync(() =>
        {
            foreach (var curItem in _currentPageViewModel.MenuItems)
            {
                var shellItem = curItem.Parent as ShellItem;
                appShell.Items.Remove(shellItem);
            }
        });
    }

    private async Task AddMenuItems()
    {
        if (_currentPageViewModel == null || _currentPageViewModel.MenuItems == null)
        {
            return;
        }

        var page = Shell.Current.CurrentPage;
        var appShell = Shell.Current as AppShell;
        await page.Dispatcher.DispatchAsync(() =>
        {
            foreach (var curItem in _currentPageViewModel.MenuItems)
            {
                appShell.Items.Add(curItem);
            }
        });
    }

    private void HomeCommandhandler()
    {
        // Go home!
    }

    private void MenuSelectionChangedCommandHandler(object obj)
    {
        var collectionView = obj as CollectionView;
        if (collectionView.SelectedItem != null)
        {
            var shellItem = collectionView.SelectedItem as ShellItem;
            var menuItem = _currentPageViewModel.MenuItems.SingleOrDefault(x => x.Text == shellItem.Title && x.Command != null);           
            menuItem?.Command.Execute(null);
            collectionView.SelectedItem = null;
        }
        else
        {
            collectionView.SelectionMode = SelectionMode.None;
        }
    }

    private void MenuSelectionPropertyChangedCommandHandler(object obj)
    {
        var collectionView = obj as CollectionView;
        if (collectionView.SelectionMode == SelectionMode.None)
        {
            collectionView.SelectionMode = SelectionMode.Single;
        }
    }
}