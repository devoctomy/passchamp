using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Windows.Views;
using System;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Windows.Graph.Windows
{
    public class PasswordEnterDialogNode : NodeBase
    {
        [NodeOutputPin(ValueType = typeof(SecureString))]
        public IDataPin<SecureString> SecurePassword
        {
            get
            {
                return GetOutput<SecureString>("SecurePassword");
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
                SecurePassword.Value = passwordEnterDialog.Password;
            }
        }
    }
}
