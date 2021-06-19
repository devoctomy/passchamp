using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public class Graph : IGraph
    {
        private string _startKey = string.Empty;
        private readonly Dictionary<INode, string> _nodeKeys;
        private readonly List<string> _executionOrder = new();

        public IReadOnlyList<string> ExecutionOrder => _executionOrder;
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
            _nodeKeys = Nodes.ToList().ToDictionary(
                x => x.Value,
                x => x.Key);
            StartKey = startKey;
        }

        public T GetNode<T>(string key) where T : INode
        {
            return Nodes.ContainsKey(key) ? (T)Nodes[key] : default;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _executionOrder.Clear();
            var startNode = GetNode<INode>(StartKey);
            await startNode.Execute(
                this,
                cancellationToken);
        }

        public void BeforeExecute(INode node)
        {
            if(_nodeKeys.TryGetValue(node, out string key))
            {
                _executionOrder.Add(key);
            }
        }
    }
}