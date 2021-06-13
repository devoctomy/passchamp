using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public interface INode
    {
        Dictionary<string, DataPin> Input { get; }
        Dictionary<string, DataPin> Output { get; }
        Task Execute(
            IGraph graph,
            CancellationToken cancellationToken);
        void PrepareInputDataPin(string key);
        void PrepareOutputDataPin(string key);
        Task ExecuteNext(
            IGraph graph,
            CancellationToken cancellationToken);

    }
}
