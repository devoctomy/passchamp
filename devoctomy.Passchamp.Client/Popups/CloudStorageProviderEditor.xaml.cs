using CommunityToolkit.Maui.Views;
using devoctomy.Passchamp.Client.Popups.Base;
using devoctomy.Passchamp.Client.ViewModels;

namespace devoctomy.Passchamp.Client.Popups;

public partial class CloudStorageProviderEditor : BasePopup<CloudStorageProviderEditorViewModel>
{
	public CloudStorageProviderEditor(CloudStorageProviderEditorViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}