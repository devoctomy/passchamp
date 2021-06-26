using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class EncryptNode : NodeBase
    {
        private const string AesAlgorithmName = "AesManaged";

        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin PlainTextBytes
        {
            get
            {
                return GetInput("PlainTextBytes");
            }
            set
            {
                Input["PlainTextBytes"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin Iv
        {
            get
            {
                return GetInput("Iv");
            }
            set
            {
                Input["Iv"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin Key
        {
            get
            {
                return GetInput("Key");
            }
            set
            {
                Input["Key"] = value;
            }
        }

        [NodeOutputPin]
        public IDataPin EncryptedBytes
        {
            get
            {
                return GetOutput("EncryptedBytes");
            }
        }

        protected override async Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            using var crypto = Aes.Create(AesAlgorithmName);
            var encryptStream = crypto.CreateEncryptor(
                    Key.GetValue<byte[]>(),
                    Iv.GetValue<byte[]>());
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(
                memoryStream,
                encryptStream,
                CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(
                PlainTextBytes.GetValue<byte[]>(),
                cancellationToken).ConfigureAwait(false);
            await cryptoStream.FlushFinalBlockAsync(cancellationToken).ConfigureAwait(false);

            EncryptedBytes.Value = memoryStream.ToArray();
        }
    }
}
