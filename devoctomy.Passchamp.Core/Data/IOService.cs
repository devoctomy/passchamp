using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Data
{
    public class IOService : IIOService
    {
        public void Delete(string path)
        {
            File.Delete(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }

        public Stream OpenNewWrite(string path)
        {
            return File.Open(
                path,
                FileMode.Create);
        }

        public async Task<string> ReadAllTextAsync(
            string path,
            CancellationToken cancellationToken)
        {
            return await File.ReadAllTextAsync(
                path,
                cancellationToken);
        }

        public async Task WriteDataAsync(
            string path,
            string contents,
            CancellationToken cancellationToken)
        {
            await File.WriteAllTextAsync(
                path,
                contents,
                cancellationToken);
        }

        public async Task WriteDataAsync(
            string path,
            byte[] contents,
            CancellationToken cancellationToken)
        {
            await File.WriteAllBytesAsync(
                path,
                contents,
                cancellationToken);
        }
    }
}
