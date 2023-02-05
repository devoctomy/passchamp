using devoctomy.Passchamp.Client.Pages.Base;
using devoctomy.Passchamp.Client.ViewModels;

namespace devoctomy.Passchamp.Client.Pages;

public partial class CloudStorageProviderEditorPage : BasePage<CloudStorageProviderEditorViewModel>
{
	public CloudStorageProviderEditorPage(CloudStorageProviderEditorViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}