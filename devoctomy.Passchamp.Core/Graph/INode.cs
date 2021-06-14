using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public interface INode
    {
        Dictionary<string, IDataPin> Input { get; }
        Dictionary<string, IDataPin> Output { get; }
        Task Execute(
            IGraph graph,
            CancellationToken cancellationToken);
        void PrepareInputDataPin(string key);
        void PrepareOutputDataPin(string key);
        Task ExecuteNext(
            IGraph graph,
            CancellationToken cancellationToken);
        IDataPin GetInput(string key);
        IDataPin GetOutput(string key);
    }
}
