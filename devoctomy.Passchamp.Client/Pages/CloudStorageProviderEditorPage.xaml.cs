using devoctomy.Passchamp.Client.Pages.Base;
using devoctomy.Passchamp.Client.ViewModels;

namespace devoctomy.Passchamp.Client.Pages;

public partial class CloudStorageProviderEditorPage : BaseContentPage<CloudStorageProviderEditorViewModel>
{
	public CloudStorageProviderEditorPage(CloudStorageProviderEditorViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}