using System.Security.Cryptography;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services
{
    public interface IRsaJsonVerifierService
    {
        Task<bool> IsApplicable(string path);
        Task<int> Verify(VerifyOptions signOptions);
        Task<bool> Verify(
            string path,
            string publicKey);
    }
}
