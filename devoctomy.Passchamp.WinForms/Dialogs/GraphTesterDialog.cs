using devoctomy.Passchamp.Models;
using devoctomy.Passchamp.Services;
using devoctomy.Passchamp.ViewModels;
using devoctomy.Passchamp.Views;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace devoctomy.Passchamp.Dialogs
{
    public partial class GraphTesterDialog : Form, IView<GraphTesterDialog, GraphTesterViewModel>
    {
        public ILogger<GraphTesterDialog> Logger { get; }
        public GraphTesterViewModel ViewModel { get; }

        public GraphTesterDialog(
            ILogger<GraphTesterDialog> logger,
            GraphTesterViewModel viewModel)
        {
            Logger = logger;
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
