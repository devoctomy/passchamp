using System;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class SCryptNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(int))]
        public IDataPin IterationCount
        {
            get
            {
                return GetInput("IterationCount");
            }
            set
            {
                Input["IterationCount"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(int))]
        public IDataPin BlockSize
        {
            get
            {
                return GetInput("BlockSize");
            }
            set
            {
                Input["BlockSize"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(int))]
        public IDataPin ThreadCount
        {
            get
            {
                return GetInput("ThreadCount");
            }
            set
            {
                Input["ThreadCount"] = value;
            }
        }

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
            var scrypt = new SCrypt(
                IterationCount.GetValue<int>(),
                BlockSize.GetValue<int>(),
                ThreadCount.GetValue<int>());
            Key.Value = scrypt.DeriveBytes(
                Password.GetValue<string>(),
                Salt.GetValue<byte[]>());
            var keyb64 = Convert.ToBase64String(Key.GetValue<byte[]>());
            return Task.CompletedTask;
        }
    }
}
