using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Services
{
    public interface IGraphLoaderService
    {
        Task<IGraph> LoadAsync(
            string graphJsonFile,
            IGraph.GraphOutputMessageDelegate outputMessage,
            CancellationToken cancellationToken);

        Task<IGraph> LoadAsync(
            Stream graphJson,
            IGraph.GraphOutputMessageDelegate outputMessage,
            CancellationToken cancellationToken);
    }
}
