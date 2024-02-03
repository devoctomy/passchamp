using devoctomy.Passchamp.Client.Pages.Base;
using devoctomy.Passchamp.Client.ViewModels;

namespace devoctomy.Passchamp.Client.Pages;

public partial class SettingsPage : BaseContentPage<SettingsViewModel>
{
	public SettingsPage(SettingsViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}