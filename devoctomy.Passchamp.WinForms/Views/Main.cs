using devoctomy.Passchamp.Dialogs;
using devoctomy.Passchamp.Models;
using devoctomy.Passchamp.Services;
using devoctomy.Passchamp.Views;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace devoctomy.Passchamp.WinForms.Views
{
    public partial class Main : Form, IView<Main, MainViewModel>
    {
        public ILogger<Main> Logger { get; }
        public MainViewModel Model { get; }
        public IViewLocator ViewLocator { get; }

        public Main(
            ILogger<Main> logger,
            IViewModelLocator viewModelLocator,
            IViewLocator viewLocator)
        {
            Logger = logger;
            Model = viewModelLocator.CreateInstance<MainViewModel>();
            ViewLocator = viewLocator;
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            using var dialog = ViewLocator.CreateInstance<GraphTesterDialog>();
            dialog.ShowDialog();
        }
    }
}
