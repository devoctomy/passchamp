using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public class NodeBase : INode
    {
        public Dictionary<string, IDataPin> Input { get; } = new Dictionary<string, IDataPin>();
        public Dictionary<string, IDataPin> Output { get; } = new Dictionary<string, IDataPin>();
        public string NextKey { get; set; }
        public bool Executed { get; protected set; }

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
            Executed = true;
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

        public IDataPin GetInput(string key)
        {
            PrepareInputDataPin(key);
            return Input[key];
        }

        public IDataPin GetOutput(string key)
        {
            PrepareOutputDataPin(key);
            return Output[key];
        }
    }
}