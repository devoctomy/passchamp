using System;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    [Obsolete("Please use DeriveKeyFromPasswordExNode due to better security.")]
    public class DeriveKeyFromPasswordNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(SecureString), DefaultValue = null)]
        public IDataPin<SecureString> SecurePassword
        {
            get
            {
                return GetInput<SecureString>("SecurePassword");
            }
            set
            {
                Input["SecurePassword"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin<byte[]> Salt
        {
            get
            {
                return GetInput<byte[]>("Salt");
            }
            set
            {
                Input["Salt"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(int), DefaultValue = 0)]
        public IDataPin<int> KeyLength
        {
            get
            {
                return GetInput<int>("KeyLength");
            }
            set
            {
                Input["KeyLength"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(int), DefaultValue = 1)]
        public IDataPin<int> IterationCount
        {
            get
            {
                return GetInput<int>("IterationCount");
            }
            set
            {
                Input["IterationCount"] = value;
            }
        }

        [NodeOutputPin(ValueType = typeof(byte[]))]
        public IDataPin<byte[]> Key
        {
            get
            {
                return GetOutput<byte[]>("Key");
            }
        }

        protected override Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            using var crypto = new System.Security.Cryptography.Rfc2898DeriveBytes(
                new NetworkCredential(null, SecurePassword.Value).Password,
                Salt.Value,
                IterationCount.Value);
            Key.Value = crypto.GetBytes(KeyLength.Value);
            return Task.CompletedTask;
        }
    }
}
