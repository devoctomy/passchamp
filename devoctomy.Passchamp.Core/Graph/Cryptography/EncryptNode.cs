using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class EncryptNode : NodeBase
    {
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

        public IDataPin EncryptedBytes
        {
            get
            {
                return GetOutput("EncryptedBytes");
            }
        }

        protected override async Task DoExecute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            using var crypto = Aes.Create("AesManaged");
            crypto.IV = Iv.GetValue<byte[]>();
            var key = Key.GetValue<byte[]>();
            crypto.KeySize = key.Length * 4;
            crypto.Key = Key.GetValue<byte[]>();

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(
                memoryStream,
                crypto.CreateEncryptor(),
                CryptoStreamMode.Write);

            var plainText = PlainTextBytes.GetValue<byte[]>();
            await cryptoStream.WriteAsync(
                plainText,
                cancellationToken);
            await cryptoStream.FlushFinalBlockAsync(cancellationToken);
            cryptoStream.Close();

            EncryptedBytes.Value = memoryStream.ToArray();
        }
    }
}
