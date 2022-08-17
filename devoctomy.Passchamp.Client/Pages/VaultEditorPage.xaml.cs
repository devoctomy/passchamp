using devoctomy.Passchamp.Client.Pages.Base;
using devoctomy.Passchamp.Client.ViewModels;

namespace devoctomy.Passchamp.Client.Pages;

public partial class VaultEditorPage : BasePage<VaultEditorViewModel>
{
	public VaultEditorPage(VaultEditorViewModel model)
		: base(model)
	{
		InitializeComponent();
	}
}