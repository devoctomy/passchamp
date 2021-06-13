using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public class NodeBase : INode
    {
        public Dictionary<string, DataPin> Input { get; } = new Dictionary<string, DataPin>();
        public Dictionary<string, DataPin> Output { get; } = new Dictionary<string, DataPin>();
        public string NextKey { get; set; }

        public virtual async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            await ExecuteNext(graph, cancellationToken);
        }

        public void PrepareInputDataPin(string key)
        {
            if (!Input.ContainsKey(key))
            {
                Input.Add(key, new DataPin(null));
            }
        }

        public void PrepareOutputDataPin(string key)
        {
            if(!Output.ContainsKey(key))
            {
                Output.Add(key, new DataPin(null));
            }
        }

        public async Task ExecuteNext(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(NextKey))
            {
                return;
            }

            var nextNode = graph.GetNode<INode>(NextKey);
            if (nextNode != null)
            {
                await nextNode.Execute(
                    graph,
                    cancellationToken);
            }
        }
    }
}