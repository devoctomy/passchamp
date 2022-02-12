using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services
{
    public class RsaJsonSignerService : IRsaJsonSignerService
    {
        public async Task<bool> IsApplicable(string path)
        {
            var jsonData = await File.ReadAllTextAsync(path).ConfigureAwait(false);
            try
            {
                JObject.Parse(jsonData);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }

        public async Task<int> Sign(SignOptions signOptions)
        {
            var privateKey = await File.ReadAllTextAsync(signOptions.KeyFile).ConfigureAwait(false);
            var signed = await Sign(
                signOptions.Input,
                privateKey);
            await File.WriteAllTextAsync(signOptions.Output, signed).ConfigureAwait(false);
            return 0;
        }

        public async Task<string> Sign(
            string path,
            string privateKey)
        {
            var jsonData = await File.ReadAllTextAsync(path);
            var json = JObject.Parse(jsonData);
            
            if(json.ContainsKey("Signature"))
            {
                json.Remove("Signature");
            }

            var jsonBytes = System.Text.Encoding.UTF8.GetBytes(json.ToString(Formatting.None));

            using var sha256Provider = SHA256.Create();
            var hashBytes = sha256Provider.ComputeHash(jsonBytes);

            using var rsaProvider = new RSACryptoServiceProvider();
            var privateKeyParams = JsonConvert.DeserializeObject<RSAParameters>(privateKey);
            rsaProvider.ImportParameters(privateKeyParams);
            var signatureBytes = rsaProvider.SignData(
                hashBytes,
                sha256Provider);
            var signatureBase64 = Convert.ToBase64String(signatureBytes);

            var signatrureJson = new JObject
            {
                { "Algorithm", new JValue("RsaJsonSigner") },
                { "Signature", new JValue(signatureBase64) }
            };
            json.Add(
                "Signature",
                signatrureJson);
            return json.ToString(Formatting.Indented);
        }
    }
}
