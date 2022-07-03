using System.IO;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Data
{
    public interface IPartialSecureJsonWriterService
    {
        public Task SaveAsync(
            object value,
            Stream stream);
    }
}
