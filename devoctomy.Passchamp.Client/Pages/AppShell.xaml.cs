using devoctomy.Passchamp.Client.Pages.Base;
using devoctomy.Passchamp.Client.ViewModels;

namespace devoctomy.Passchamp.Client
{
    public partial class AppShell : BaseShell<AppShellViewModel>
    {
        public AppShell(AppShellViewModel model)
            : base(model)
        {
            InitializeComponent();
        }
    }
}