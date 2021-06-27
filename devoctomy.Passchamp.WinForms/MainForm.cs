using devoctomy.Passchamp.Core.Graph.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace devoctomy.Passchamp.WinForms
{
    public partial class MainForm : Form
    {
        private readonly ILogger<MainForm> _logger;
        private readonly IGraphLoaderService _graphLoaderService;

        public MainForm(
            ILogger<MainForm> logger,
            IGraphLoaderService graphLoaderService)
        {
            _logger = logger;
            _graphLoaderService = graphLoaderService;

            InitializeComponent();
        }

        private async Task TestLoad(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Loading graph from json...");
            using var inputStream = File.OpenRead("Configuration/Graph/default.json");
            var graph = await _graphLoaderService.LoadAsync(
                inputStream,
                cancellationToken);
            _logger.LogInformation("Graph loaded.");
        }

        private async void button1_Click(object sender, System.EventArgs e)
        {
            await TestLoad(CancellationToken.None);
        }
    }
}
