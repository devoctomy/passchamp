using devoctomy.Passchamp.Client.Pages.Base;
using devoctomy.Passchamp.Client.ViewModels;

namespace devoctomy.Passchamp.Client
{
    public partial class AppShellPage : BaseShell<AppShellViewModel>
    {
        public AppShellPage(AppShellViewModel model)
            : base(model)
        {
            InitializeComponent();
        }
    }
}