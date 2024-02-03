using devoctomy.Passchamp.Client.Pages.Base;
using devoctomy.Passchamp.Client.ViewModels;

namespace devoctomy.Passchamp.Client.Pages;

public partial class ThemeTestPage : BaseContentPage<ThemeTestViewModel>
{
	public ThemeTestPage(ThemeTestViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}