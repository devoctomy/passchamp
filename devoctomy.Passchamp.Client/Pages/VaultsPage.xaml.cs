using devoctomy.Passchamp.Client.Pages.Base;
using devoctomy.Passchamp.Client.ViewModels;

namespace devoctomy.Passchamp.Client.Pages;

public partial class VaultsPage : BaseContentPage<VaultsViewModel>
{
	public VaultsPage(VaultsViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}