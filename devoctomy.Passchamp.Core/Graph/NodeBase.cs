using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public class NodeBase : INode
    {
        private List<string> _inputPinNames;
        private List<string> _outputPinNames;

        public Dictionary<string, IDataPin> Input { get; } = new Dictionary<string, IDataPin>();
        public Dictionary<string, IDataPin> Output { get; } = new Dictionary<string, IDataPin>();
        public string NextKey { get; set; }
        public bool Executed { get; protected set; }

        public NodeBase()
        {
            var curType = GetType();
            var inputProperties = curType.GetProperties().Where(prop => prop.IsDefined(typeof(NodeInputPinAttribute), false)).ToList();
            _inputPinNames = inputProperties.Select(x => x.Name).ToList();
            var outputProperties = curType.GetProperties().Where(prop => prop.IsDefined(typeof(NodeOutputPinAttribute), false)).ToList();
            _outputPinNames = outputProperties.Select(x => x.Name).ToList();
        }

        public void PrepareInputDataPin(
            string key,
            bool validate = true)
        {
            if(validate && !_inputPinNames.Contains(key))
            {
                throw new KeyNotFoundException($"Input pin with the key name '{key}' not found on node type {GetType().Name}.");
            }

            if (!Input.ContainsKey(key))
            {
                Input.Add(key, new DataPin(null));
            }
        }

        public void PrepareOutputDataPin(
            string key,
            bool validate = true)
        {
            if (validate && !_outputPinNames.Contains(key))
            {
                throw new KeyNotFoundException($"Input pin with the key name '{key}' not found on node type {GetType().Name}.");
            }

            if (!Output.ContainsKey(key))
            {
                Output.Add(key, new DataPin(null));
            }
        }

        public async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            graph.BeforeExecute(this);
            await DoExecute(
                graph,
                cancellationToken);
            await ExecuteNext(
                graph,
                cancellationToken);
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

        protected virtual Task DoExecute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
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