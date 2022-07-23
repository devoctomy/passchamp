using devoctomy.Passchamp.SignTool.Services.Enums;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services
{
    public class JsonWebKeyRsaKeyGeneratorService : IRsaKeyGeneratorService
    {
        public void Generate(
            int keySize,
            out string privateKey,
            out string publicKey)
        {
            using var rsaProvider = new RSACryptoServiceProvider(keySize);
            var privateKeyParams = rsaProvider.ExportParameters(true);
            var publicKeyParams = rsaProvider.ExportParameters(false);

            if (keySize != 2048 && keySize != 4096)
            {
                throw new NotImplementedException($"Key size of '{keySize}' not supported for JsonWebKeyRsaKeyGeneratorService.");
            }

            var id = Guid.NewGuid().ToString();
            var privateSecurityKey = new RsaSecurityKey(privateKeyParams);
            var privateJwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(privateSecurityKey);
            privateJwk.KeyId = id;
            privateJwk.Use = "sig";
            privateJwk.Alg = $"RSA-{keySize}-Sign";
            var publicSecurityKey = new RsaSecurityKey(publicKeyParams);
            var publicJwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(publicSecurityKey);
            publicJwk.KeyId = id;
            publicJwk.Use = "sig";
            publicJwk.Alg = $"RSA-{keySize}-Sign";
            privateKey = JsonConvert.SerializeObject(privateJwk);
            publicKey = JsonConvert.SerializeObject(publicJwk);
        }
    }
}
