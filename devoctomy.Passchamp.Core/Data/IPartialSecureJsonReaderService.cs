using System.IO;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Data
{
    public interface IPartialSecureJsonReaderService
    {
        public Task<T> LoadAsync<T>(Stream stream);
    }
}