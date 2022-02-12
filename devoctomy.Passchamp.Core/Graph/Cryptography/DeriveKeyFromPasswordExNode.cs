using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class DeriveKeyFromPasswordExNode : NodeBase
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
            IntPtr ptr = Marshal.SecureStringToBSTR(SecurePassword.Value);
            try
            {
                byte[] passwordByteArray = null;
                int length = Marshal.ReadInt32(ptr, -4);
                passwordByteArray = new byte[length];
                GCHandle handle = GCHandle.Alloc(passwordByteArray, GCHandleType.Pinned);
                try
                {
                    for (int i = 0; i < length; i++)
                    {
                        passwordByteArray[i] = Marshal.ReadByte(ptr, i);
                    }
                    using (var rfc2898 = new System.Security.Cryptography.Rfc2898DeriveBytes(
                        passwordByteArray,
                        Salt.Value,
                        IterationCount.Value))
                    {
                        Key.Value = rfc2898.GetBytes(KeyLength.Value);
                    }
                }
                finally
                {
                    Array.Clear(passwordByteArray, 0, passwordByteArray.Length);
                    handle.Free();
                }
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }

            return Task.CompletedTask;
        }
    }
}
