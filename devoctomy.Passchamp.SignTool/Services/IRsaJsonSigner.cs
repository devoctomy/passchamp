using System.Security.Cryptography;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services
{
    public interface IRsaJsonSigner
    {
        Task<bool> IsApplicable(string path);
        Task<string> Sign(
            string path,
            RSAParameters key);
    }
}
