using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class SCryptExNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(int))]
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

        [NodeInputPin(ValueType = typeof(int))]
        public IDataPin<int> BlockSize
        {
            get
            {
                return GetInput<int>("BlockSize");
            }
            set
            {
                Input["BlockSize"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(int))]
        public IDataPin<int> ThreadCount
        {
            get
            {
                return GetInput<int>("ThreadCount");
            }
            set
            {
                Input["ThreadCount"] = value;
            }
        }

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

                    var scrypt = new SCrypt(
                        IterationCount.Value,
                        BlockSize.Value,
                        ThreadCount.Value);
                    Key.Value = scrypt.DeriveBytes(
                        passwordByteArray,
                        Salt.Value);
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
