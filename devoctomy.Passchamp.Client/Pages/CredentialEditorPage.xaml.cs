using devoctomy.Passchamp.Client.Pages.Base;
using devoctomy.Passchamp.Client.ViewModels;

namespace devoctomy.Passchamp.Client.Pages;

public partial class CredentialEditorPage : BaseContentPage<CredentialEditorViewModel>
{
	public CredentialEditorPage(CredentialEditorViewModel model)
        : base(model)
    {
		InitializeComponent();
	}
}