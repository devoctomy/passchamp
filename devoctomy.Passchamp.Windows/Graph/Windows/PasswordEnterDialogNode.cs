using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Windows.Views;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Windows.Graph.Windows
{
    public class PasswordEnterDialogNode : NodeBase
    {
        [NodeOutputPin(ValueType = typeof(string))]
        public IDataPin<string> Password
        {
            get
            {
                return GetOutput<string>("Password");
            }
        }

        protected override async Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            if(!graph.ExtendedParams.ContainsKey("dispatcher"))
            {
                throw new InvalidOperationException("Extended parameter 'dispatcher' must be set prior to execution.");
            }

            var dispatcher = (System.Windows.Threading.Dispatcher)graph.ExtendedParams["dispatcher"];
            await dispatcher.InvokeAsync(DoWork);
        }

        private void DoWork()
        {
            var passwordEnterDialog = new PasswordEnterDialog();
            var result = passwordEnterDialog.ShowDialog();
            if (result.GetValueOrDefault())
            {
                var networkCredential = new System.Net.NetworkCredential(string.Empty, passwordEnterDialog.Password);
                Password.Value = networkCredential.Password;
            }
        }


    }
}
