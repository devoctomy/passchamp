using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class DecryptNode : NodeBase
    {
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

        public IDataPin DecryptedBytes
        {
            get
            {
                return GetOutput("DecryptedBytes");
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

            using var memoryStream = new MemoryStream(Cipher.GetValue<byte[]>());
            using var cryptoStream = new CryptoStream(
                memoryStream,
                crypto.CreateDecryptor(),
                CryptoStreamMode.Read);
            using var output = new MemoryStream();
            await cryptoStream.CopyToAsync(output);

            DecryptedBytes.Value = output.ToArray();
        }
    }
}
