using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class DecryptNode : NodeBase
    {
        private const string AesAlgorithmName = "AesManaged";

        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin Cipher
        {
            get
            {
                return GetInput("Cipher");
            }
            set
            {
                Input["Cipher"] = value;
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
        public IDataPin DecryptedBytes
        {
            get
            {
                return GetOutput("DecryptedBytes");
            }
        }

        protected override async Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            using var crypto = Aes.Create(AesAlgorithmName);
            using var decryptStream = crypto.CreateDecryptor(
                    Key.GetValue<byte[]>(),
                    Iv.GetValue<byte[]>());
            using var cryptoStream = new CryptoStream(
                new MemoryStream(Cipher.GetValue<byte[]>()),
                decryptStream,
                CryptoStreamMode.Read);
            using var output = new MemoryStream();
            await cryptoStream.CopyToAsync(
                output,
                cancellationToken).ConfigureAwait(false);

            DecryptedBytes.Value = output.ToArray();
        }
    }
}
