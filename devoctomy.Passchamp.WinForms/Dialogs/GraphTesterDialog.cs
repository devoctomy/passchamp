using devoctomy.Passchamp.Models;
using devoctomy.Passchamp.Services;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace devoctomy.Passchamp.Dialogs
{
    public partial class GraphTesterDialog : Form
    {
        private readonly ILogger<GraphTesterDialog> _logger;
        private readonly GraphTesterViewModel _viewModel;

        public GraphTesterDialog(
            ILogger<GraphTesterDialog> logger,
            IViewModelLocator viewModelLocator)
        {
            _logger = logger;
            _viewModel = viewModelLocator.CreateInstance<GraphTesterViewModel>();
            InitializeComponent();
        }
    }
}
