using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services
{
    public class RsaJsonSigner : IRsaJsonSigner
    {
        public async Task<bool> IsApplicable(string path)
        {
            var jsonData = await File.ReadAllTextAsync(path);
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

        public async Task<string> Sign(
            string path,
            RSAParameters key)
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
            rsaProvider.ImportParameters(key);
            var signatureBytes = rsaProvider.SignData(
                hashBytes,
                sha256Provider);
            var signatureBase64 = Convert.ToBase64String(signatureBytes);

            var signatrureJson = new JObject();
            signatrureJson.Add("Algorithm", new JValue("RsaJsonSigner"));
            signatrureJson.Add("Signature", new JValue(signatureBase64));
            json.Add(
                "Signature",
                signatrureJson);
            return json.ToString(Formatting.Indented);
        }
    }
}
