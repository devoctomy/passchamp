namespace devoctomy.Passchamp.SignTool.Services
{
    public class RsaKeyPair
    {
        public string PublicKey { get; }
        public string PrivateKey { get; }

        public RsaKeyPair(string publicKey, string privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
    }
}
