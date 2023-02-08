using Newtonsoft.Json;
using System.Security.Cryptography;

namespace devoctomy.Passchamp.SignTool.Services;

public class RsaKeyGeneratorService : IRsaKeyGeneratorService
{
    public RsaKeyPair Generate(int keySize)
    {
        using var rsaProvider = new RSACryptoServiceProvider(keySize);
        var privateKeyParams = rsaProvider.ExportParameters(true);
        var publicKeyParams = rsaProvider.ExportParameters(false);
        return new RsaKeyPair(
            JsonConvert.SerializeObject(publicKeyParams),
            JsonConvert.SerializeObject(privateKeyParams));
    }
}
