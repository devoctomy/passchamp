using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cryptography
{
    public interface IWordListLoader
    {
        Task<Dictionary<string, List<string>>> LoadAllAsync(CancellationToken cancellationToken);
    }
}
