using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Windows.Extensions;
using devoctomy.Passchamp.Windows.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace devoctomy.Passchamp.Windows.ViewModels
{
    public class GraphTesterViewModel : ViewModelBase<GraphTesterModel>
    {
        private readonly IGraphLoaderService _graphLoaderService;

        public ICommand GraphBrowse { get; }
        public ICommand Execute { get; }

        public GraphTesterViewModel(
            ILogger<GraphTesterViewModel> logger,
            GraphTesterModel model,
            IGraphLoaderService graphLoaderService)
            : base(logger, model)
        {
            Execute = new RelayCommand(DoExecute);
            GraphBrowse = new RelayCommand(DoGraphBrowse);
            _graphLoaderService = graphLoaderService;
        }

        private async void DoExecute()
        {
            if(Model.Graph != null)
            {
                await Model.Graph.ExecuteAsync(CancellationToken.None);
            }
        }

        private async void DoGraphBrowse()
        {
            Logger.LogInformation("Browsing for graph file...");
            var openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".json",
                Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
                Multiselect = false,
                InitialDirectory = Environment.CurrentDirectory + $"{Path.DirectorySeparatorChar}Config{Path.DirectorySeparatorChar}Graphs",
                AddExtension = true
            };
            if(openFileDialog.ShowDialog() == true)
            {
                var stream = openFileDialog.OpenFile();
                Model.Graph = await _graphLoaderService.LoadAsync(
                    stream,
                    OutputMessage,
                    CancellationToken.None);
            }
        }

        private void OutputMessage(INode node, string message)
        {
            Model.Messages.Add(message);
        }
    }
}
