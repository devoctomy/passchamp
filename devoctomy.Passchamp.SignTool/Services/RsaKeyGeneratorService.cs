using Newtonsoft.Json;
using System;
using System.Security.Cryptography;

namespace devoctomy.Passchamp.SignTool.Services
{
    public class RsaKeyGeneratorService : IRsaKeyGeneratorService
    {
        public void Generate(
            int keySize,
            out string privateKey,
            out string publicKey)
        {
            using var rsaProvider = new RSACryptoServiceProvider(keySize);
            var privateKeyParams = rsaProvider.ExportParameters(true);
            var publicKeyParams = rsaProvider.ExportParameters(false);
            privateKey = JsonConvert.SerializeObject(privateKeyParams);
            publicKey = JsonConvert.SerializeObject(publicKeyParams);
        }
    }
}
