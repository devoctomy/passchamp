using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Windows.Model;
using devoctomy.Passchamp.Windows.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.IO;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace devoctomy.Passchamp.Windows.ViewModels
{
    public class GraphTesterViewModel : ViewModelBase<GraphTesterModel>
    {
        private readonly IGraphLoaderService _graphLoaderService;
        private readonly IFileDialogService _fileDialogService;

        public ICommand GraphBrowse { get; }
        public ICommand Execute { get; }

        public GraphTesterViewModel(
            ILogger<GraphTesterViewModel> logger,
            GraphTesterModel model,
            IGraphLoaderService graphLoaderService,
            IFileDialogService fileDialogService)
            : base(logger, model)
        {
            Execute = new RelayCommand(DoExecute);
            GraphBrowse = new RelayCommand(DoGraphBrowse);
            _graphLoaderService = graphLoaderService;
            _fileDialogService = fileDialogService;
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
            var success = _fileDialogService.OpenFile(new OpenFileDialogOptions
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".json",
                Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
                Multiselect = false,
                InitialDirectory = Environment.CurrentDirectory + $"{Path.DirectorySeparatorChar}Config{Path.DirectorySeparatorChar}Graphs",
                AddExtension = true
            }, out var fileName);

            if (success.GetValueOrDefault())
            {
                Model.Graph = await _graphLoaderService.LoadAsync(
                    fileName,
                    OutputMessage,
                    CancellationToken.None);
                Model.Graph.ExtendedParams.Add("dispatcher", Dispatcher);
            }
        }

        private void OutputMessage(INode node, string message)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var sourceKey = node != null ? $"{Model.Graph.NodeKeys[node]} ({node.GetType().Name})" : "Graph";
                Model.Messages.Add($"{sourceKey} ::  {message}");
            }));
        }
    }
}
