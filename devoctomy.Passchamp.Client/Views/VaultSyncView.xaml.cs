using devoctomy.Passchamp.Client.ViewModels;
using devoctomy.Passchamp.Client.Views.Base;

namespace devoctomy.Passchamp.Client.Views;

public partial class VaultSyncView : BaseView<VaultSyncViewModel>
{
    public VaultSyncView(VaultSyncViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}