using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public interface INode
    {
        Dictionary<string, IDataPin> Input { get; }
        Dictionary<string, IDataPin> Output { get; }
        public string NextKey { get; set; }
        public bool Executed { get; }
        Task Execute(
            IGraph graph,
            CancellationToken cancellationToken);
        void PrepareInputDataPin(
            string key,
            bool validate = true);
        void PrepareOutputDataPin(
            string key,
            bool validate = true);
        Task ExecuteNext(
            IGraph graph,
            CancellationToken cancellationToken);
        IDataPin GetInput(string key);
        IDataPin GetOutput(string key);
    }
}
