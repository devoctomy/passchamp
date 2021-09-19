using System.Security.Cryptography;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services
{
    public interface IRsaJsonVerifier
    {
        Task<bool> IsApplicable(string path);
        Task Verify(
            string path,
            RSAParameters key);
    }
}
