using devoctomy.Passchamp.Models;
using devoctomy.Passchamp.Services;
using devoctomy.Passchamp.Views;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace devoctomy.Passchamp.Dialogs
{
    public partial class GraphTesterDialog : Form, IView<GraphTesterDialog, GraphTesterViewModel> //NOSONAR
    {
        public ILogger<GraphTesterDialog> Logger { get; }
        public GraphTesterViewModel Model { get; }

        public GraphTesterDialog(
            ILogger<GraphTesterDialog> logger,
            IViewModelLocator viewModelLocator)
        {
            Logger = logger;
            Model = viewModelLocator.CreateInstance<GraphTesterViewModel>();
            InitializeComponent();
        }
    }
}
