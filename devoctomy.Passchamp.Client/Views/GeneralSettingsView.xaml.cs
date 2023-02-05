using devoctomy.Passchamp.Client.ViewModels;
using devoctomy.Passchamp.Client.Views.Base;

namespace devoctomy.Passchamp.Client.Views;

public partial class GeneralSettingsView : BaseView<GeneralSettingsViewModel>
{
    public GeneralSettingsView(GeneralSettingsViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}