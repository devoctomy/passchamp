using CommunityToolkit.Mvvm.ComponentModel;
using devoctomy.Passchamp.Client.ViewModels.Base;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public partial class GeneralSettingsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string theme;

        public GeneralSettingsViewModel()
        {
            this.PropertyChanged += GeneralSettingsViewModel_PropertyChanged;
        }

        private void GeneralSettingsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(GeneralSettingsViewModel.Theme):
                    {
                        App.Current.UserAppTheme =
                            Theme == "System" ?
                            Application.Current.PlatformAppTheme :
                            Enum.Parse<AppTheme>(Theme);
                        break;
                    }

                default:
                    {
                        // Do nothing!
                        break;
                    }
            }
        }
    }
}
