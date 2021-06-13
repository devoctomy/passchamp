using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public class Graph : IGraph
    {
        private string _startKey = string.Empty;

        public Dictionary<string, NodeBase> Nodes { get; }
        public string StartKey
        { 
            get
            {
                return _startKey;
            }
            private set
            {
                if(!Nodes.ContainsKey(value))
                {
                    throw new KeyNotFoundException($"Node with key {value} not found.");
                }

                _startKey = value;
            }
        }

        public Graph(
            Dictionary<string, NodeBase> nodes,
            string startKey)
        {
            Nodes = nodes;
            StartKey = startKey;
        }

        public T GetNode<T>(string key) where T : NodeBase
        {
            return Nodes.ContainsKey(key) ? (T)Nodes[key] : null;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await GetNode<NodeBase>(StartKey).Execute(
                this,
                cancellationToken);
        }
    }
}