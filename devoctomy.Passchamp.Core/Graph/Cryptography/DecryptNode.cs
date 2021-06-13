using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class DecryptNode : NodeBase
    {
        public DataPin Cipher
        {
            get
            {
                return Input["Cipher"];
            }
            set
            {
                Input["Cipher"] = value;
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

        public DataPin DecryptedBytes
        {
            get
            {
                PrepareOutputDataPin("DecryptedBytes");
                return Output["DecryptedBytes"];
            }
            set
            {
                Output["DecryptedBytes"] = value;
            }
        }

        public override async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            using var crypto = Aes.Create("AesManaged");
            crypto.IV = Iv.GetValue<byte[]>();
            var key = Key.GetValue<byte[]>();
            crypto.KeySize = key.Length * 4;
            crypto.Key = Key.GetValue<byte[]>();

            using var memoryStream = new MemoryStream(Cipher.GetValue<byte[]>());
            using var cryptoStream = new CryptoStream(
                memoryStream,
                crypto.CreateDecryptor(),
                CryptoStreamMode.Read);
            using var output = new MemoryStream();
            await cryptoStream.CopyToAsync(output);

            DecryptedBytes.Value = output.ToArray();

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}
