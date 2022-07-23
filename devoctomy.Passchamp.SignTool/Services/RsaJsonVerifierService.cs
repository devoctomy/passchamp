using devoctomy.Passchamp.SignTool.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services
{
    public class RsaJsonVerifierService : IRsaJsonVerifierService
    {
        public async Task<bool> IsApplicable(string path)
        {
            var jsonData = await File.ReadAllTextAsync(path).ConfigureAwait(false);
            var json = JObject.Parse(jsonData);

            if(!json.ContainsKey("Signature"))
            {
                return false;
            }

            var signature = (JObject)json["Signature"];
            if (!signature.ContainsKey("Algorithm") ||
                signature["Algorithm"].Value<string>() != "RsaJsonSigner" ||
                !signature.ContainsKey("Signature"))
            {
                return false;
            }

            return true;
        }

        public async Task<int> Verify(VerifyOptions verifyOptions)
        {
            var result = await Verify(
                verifyOptions.Input,
                await File.ReadAllTextAsync(verifyOptions.KeyFile).ConfigureAwait(false));
            return result ? (int)ErrorCodes.Success : (int)ErrorCodes.VerificationFailed;
        }

        public async Task<bool> Verify(
            string path,
            string publicKey)
        {
            var jsonData = await File.ReadAllTextAsync(path);
            var json = JObject.Parse(jsonData);

            var signatureBase64 = json["Signature"]["Signature"].Value<string>();
            var signature = Convert.FromBase64String(signatureBase64);

            json.Remove("Signature");
            using var sha256Provider = SHA256.Create();
            var unsignedBytes = sha256Provider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(json.ToString(Newtonsoft.Json.Formatting.None)));

            using var rsaProvider = new RSACryptoServiceProvider();
            var publicKeyParams = JsonConvert.DeserializeObject<RSAParameters>(publicKey);
            rsaProvider.ImportParameters(publicKeyParams);
            return rsaProvider.VerifyData(unsignedBytes, sha256Provider, signature);
        }
    }
}
