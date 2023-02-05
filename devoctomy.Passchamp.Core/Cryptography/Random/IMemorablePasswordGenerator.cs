using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cryptography.Random;

public interface IMemorablePasswordGenerator
{
    Task<string> GenerateAsync(
        string pattern,
        CancellationToken cancellationToken);
}
