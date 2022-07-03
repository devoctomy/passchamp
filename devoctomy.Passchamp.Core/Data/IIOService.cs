using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Data
{
    public interface IIOService
    {
        public bool Exists(string path);
        public void Delete(string path);
        public Task<string> ReadAllTextAsync(
            string path,
            CancellationToken cancellationToken);
        public Task WriteDataAsync(
            string path,
            string contents,
            CancellationToken cancellationToken);
        public Task WriteDataAsync(
            string path,
            byte[] contents,
            CancellationToken cancellationToken);
        public Stream OpenRead(string path);
    }
}
