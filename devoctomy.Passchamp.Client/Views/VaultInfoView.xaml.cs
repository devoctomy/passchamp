using devoctomy.Passchamp.Client.ViewModels;
using devoctomy.Passchamp.Client.Views.Base;

namespace devoctomy.Passchamp.Client.Views;

public partial class VaultInfoView : BaseView<VaultInfoViewModel>
{
	public VaultInfoView(VaultInfoViewModel viewModel)
        : base(viewModel)
	{
		InitializeComponent();
	}
}