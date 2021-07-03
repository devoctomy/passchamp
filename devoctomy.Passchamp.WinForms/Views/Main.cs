using devoctomy.Passchamp.Dialogs;
using devoctomy.Passchamp.Services;
using devoctomy.Passchamp.ViewModels;
using devoctomy.Passchamp.Views;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace devoctomy.Passchamp.WinForms.Views
{
    public partial class Main : Form, IView<Main, MainViewModel>
    {
        public ILogger<Main> Logger { get; }
        public MainViewModel ViewModel { get; }

        public Main(
            ILogger<Main> logger,
            MainViewModel mainViewModel)
        {
            Logger = logger;
            ViewModel = mainViewModel;
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            ViewModel.DisplayGraphTesterDialog();
        }
    }
}
