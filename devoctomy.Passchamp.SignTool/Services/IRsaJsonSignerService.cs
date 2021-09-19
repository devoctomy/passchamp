using System.Security.Cryptography;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services
{
    public interface IRsaJsonSignerService
    {
        Task<bool> IsApplicable(string path);
        Task<string> Sign(
            string path,
            string privateKey);
    }
}
