using devoctomy.Passchamp.Core.Data;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Config
{
    public interface IApplicationConfigLoaderService<T>
    {
        T Config { get; }
        Task LoadAsync(CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
