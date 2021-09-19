using System.Security.Cryptography;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services
{
    public interface IRsaJsonVerifierService
    {
        Task<bool> IsApplicable(string path);
        Task<bool> Verify(
            string path,
            string publicKey);
    }
}
