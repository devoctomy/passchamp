using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public class Graph : IGraph
    {
        private string _startKey = string.Empty;

        public Dictionary<string, INode> Nodes { get; }
        public int ExecutedNodeCount { get; set; }
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
            Dictionary<string, INode> nodes,
            string startKey)
        {
            Nodes = nodes;
            StartKey = startKey;
        }

        public T GetNode<T>(string key) where T : INode
        {
            return Nodes.ContainsKey(key) ? (T)Nodes[key] : default(T);
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var startNode = GetNode<INode>(StartKey);
            await startNode.Execute(
                this,
                cancellationToken);
        }
    }
}