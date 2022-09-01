using devoctomy.Passchamp.Client.ViewModels;
using devoctomy.Passchamp.Client.Views.Base;

namespace devoctomy.Passchamp.Client.Views;

public partial class CloudSettings : BaseView<SettingsViewModel>
{
	public CloudSettings(SettingsViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}