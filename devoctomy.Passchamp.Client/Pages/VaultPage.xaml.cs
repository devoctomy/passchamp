using devoctomy.Passchamp.Client.Pages.Base;
using devoctomy.Passchamp.Client.ViewModels;

namespace devoctomy.Passchamp.Client.Pages;

public partial class VaultPage : BaseContentPage<VaultViewModel>
{
    public VaultPage(VaultViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}