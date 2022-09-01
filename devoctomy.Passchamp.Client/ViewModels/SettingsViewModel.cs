using CommunityToolkit.Mvvm.ComponentModel;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Maui.Services;
using System.Windows.Input;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public partial class SettingsViewModel : BaseAppShellPageViewModel
    {
        [ObservableProperty]
        public CloudSettingsViewModel cloudSettings;

        public ICommand AcceptCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand TabChanged { get; }


        private readonly IShellNavigationService _shellNavigationService;
        private readonly static SemaphoreSlim _loaderLock = new(1, 1);
        private bool _loaded = false;

        public SettingsViewModel(
            CloudSettingsViewModel cloudSettingsViewModel,
            IShellNavigationService shellNavigationService)
        {
            CloudSettings = cloudSettingsViewModel;

            AcceptCommand = new Command(AcceptCommandHandler);
            CancelCommand = new Command(CancelCommandHandler);
            TabChanged = new Command(TabChangedHandler);
            _shellNavigationService = shellNavigationService;
            SetupMenuItems();
        }

        public override async Task OnAppearingAsync()
        {
            await _loaderLock.WaitAsync();
            try
            {
                if (!_loaded)
                {
                    await cloudSettings.Init();
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

        public override void OnSetupMenuItems()
        {
            MenuItems.Add(new MenuItem
            {
                Text = "Accept",
                Command = AcceptCommand
            });
            MenuItems.Add(new MenuItem
            {
                Text = "Cancel",
                Command = CancelCommand
            });
        }

        private void CancelCommandHandler(object param)
        {
            _shellNavigationService.GoBackAsync();
        }

        private void AcceptCommandHandler(object param)
        {
            _shellNavigationService.GoToAsync("//Vaults");
        }

        private void TabChangedHandler(object param)
        {
        }
    }
}
