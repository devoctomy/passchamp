using System.IO;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Data;

public interface IPartialSecureJsonWriterService
{
    public void RemoveAll(object value);
    public Task SaveAsync(
        object value,
        Stream stream);
}
