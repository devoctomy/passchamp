using devoctomy.Passchamp.Client.Pages.Base;
using devoctomy.Passchamp.Client.ViewModels;

namespace devoctomy.Passchamp.Client.Pages;

public partial class EnterMasterPassphrasePage : BaseContentPage<EnterMasterPassphraseViewModel>
{
	public EnterMasterPassphrasePage(EnterMasterPassphraseViewModel model)
        : base(model)
    {
		InitializeComponent();
	}
}