using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public class NodeBase
    {
        public Dictionary<string, DataPin> Input { get; } = new Dictionary<string, DataPin>();
        public Dictionary<string, DataPin> Output { get; } = new Dictionary<string, DataPin>();
        public string NextKey { get; set; }

        public virtual async Task Execute(
            Graph graph,
            CancellationToken cancellationToken)
        {
            await ExecuteNext(graph, cancellationToken);
        }

        protected void PrepareInputDataPin(string key)
        {
            if (!Input.ContainsKey(key))
            {
                Input.Add(key, new DataPin(null));
            }
        }

        protected void PrepareOutputDataPin(string key)
        {
            if(!Output.ContainsKey(key))
            {
                Output.Add(key, new DataPin(null));
            }
        }

        protected async Task ExecuteNext(
            Graph graph,
            CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(NextKey))
            {
                return;
            }

            var nextNode = graph.GetNode<NodeBase>(NextKey);
            if (nextNode != null)
            {
                await nextNode.Execute(
                    graph,
                    cancellationToken);
            }
        }
    }
}