using devoctomy.Passchamp.Client.ViewModels;
using devoctomy.Passchamp.Client.Views.Base;

namespace devoctomy.Passchamp.Client.Views;

public partial class CloudSettingsView : BaseView<CloudSettingsViewModel>
{
	public CloudSettingsView(CloudSettingsViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}