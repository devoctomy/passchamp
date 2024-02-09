using devoctomy.Passchamp.Client.ViewModels;
using devoctomy.Passchamp.Client.Views.Base;

namespace devoctomy.Passchamp.Client.Views;

public partial class VaultSecurityView : BaseView<VaultSecurityViewModel>
{
	public VaultSecurityView(VaultSecurityViewModel viewModel)
        : base(viewModel)
    {
		InitializeComponent();
	}
}