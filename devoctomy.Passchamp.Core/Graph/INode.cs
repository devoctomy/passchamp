using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public interface INode
    {
        Dictionary<string, PropertyInfo> InputPinsProperties { get; }
        Dictionary<string, PropertyInfo> OutputPinsProperties { get; }
        Dictionary<string, IDataPin> Input { get; }
        Dictionary<string, IDataPin> Output { get; }
        public string NextKey { get; set; }
        public bool Executed { get; }
        Task ExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken);
        void PrepareInputDataPin(
            string key,
            bool validate = true);
        void PrepareOutputDataPin(
            string key,
            bool validate = true);
        Task ExecuteNextAsync(
            IGraph graph,
            CancellationToken cancellationToken);
        IDataPin GetInput(string key);
        IDataPin GetOutput(string key);
    }
}
