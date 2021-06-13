using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class EncryptNode : NodeBase
    {
        public DataPin PlainTextBytes
        {
            get
            {
                return Input["PlainTextBytes"];
            }
            set
            {
                Input["PlainTextBytes"] = value;
            }
        }

        public DataPin Iv
        {
            get
            {
                return Input["Iv"];
            }
            set
            {
                Input["Iv"] = value;
            }
        }

        public DataPin Key
        {
            get
            {
                return Input["Key"];
            }
            set
            {
                Input["Key"] = value;
            }
        }

        public DataPin EncryptedBytes
        {
            get
            {
                PrepareOutputDataPin("EncryptedBytes");
                return Output["EncryptedBytes"];
            }
            set
            {
                Output["EncryptedBytes"] = value;
            }
        }

        public override async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            using var crypto = Aes.Create("AesManaged");
            crypto.IV = Iv.Value as byte[];
            var key = Key.Value as byte[];
            crypto.KeySize = key.Length * 4;
            crypto.Key = Key.Value as byte[];

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(
                memoryStream,
                crypto.CreateEncryptor(),
                CryptoStreamMode.Write);

            var plainText = PlainTextBytes.Value as byte[];
            await cryptoStream.WriteAsync(
                plainText,
                0,
                plainText.Length,
                cancellationToken);
            await cryptoStream.FlushFinalBlockAsync(cancellationToken);
            cryptoStream.Close();

            EncryptedBytes.Value = memoryStream.ToArray();

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}
