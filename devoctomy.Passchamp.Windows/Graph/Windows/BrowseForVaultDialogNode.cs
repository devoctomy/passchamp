using devoctomy.Passchamp.Core.Graph;
using Microsoft.Win32;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Windows.Graph.Windows
{
    public class BrowseForVaultDialogNode : NodeBase
    {
        [NodeOutputPin(ValueType = typeof(string))]
        public IDataPin<string> FileName
        {
            get
            {
                return GetOutput<string>("FileName");
            }
        }

        protected override async Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            if (!graph.ExtendedParams.ContainsKey("dispatcher"))
            {
                throw new InvalidOperationException("Extended parameter 'dispatcher' must be set prior to execution.");
            }

            var dispatcher = (System.Windows.Threading.Dispatcher)graph.ExtendedParams["dispatcher"];
            await dispatcher.InvokeAsync(DoWork);
        }

        private void DoWork()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "vault files (*.vault)|*.vault|All files (*.*)|*.*"
            };
            var result = openFileDialog.ShowDialog();
            if (result.GetValueOrDefault())
            {
                FileName.Value = openFileDialog.FileName;
            }
        }
    }
}
