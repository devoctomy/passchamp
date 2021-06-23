using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class DeriveKeyFromPasswordNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(string), DefaultValue = "")]
        public IDataPin Password
        {
            get
            {
                return GetInput("Password");
            }
            set
            {
                Input["Password"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin Salt
        {
            get
            {
                return GetInput("Salt");
            }
            set
            {
                Input["Salt"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(int), DefaultValue = 0)]
        public IDataPin KeyLength
        {
            get
            {
                return GetInput("KeyLength");
            }
            set
            {
                Input["KeyLength"] = value;
            }
        }

        [NodeOutputPin]
        public IDataPin Key
        {
            get
            {
                return GetOutput("Key");
            }
        }

        protected override Task DoExecute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            using var crypto = new System.Security.Cryptography.Rfc2898DeriveBytes(
                Password.GetValue<string>(),
                Salt.GetValue<byte[]>());
            Key.Value = crypto.GetBytes(KeyLength.GetValue<int>());
            return Task.CompletedTask;
        }
    }
}
