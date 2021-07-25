using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
    public class NodeBase : INode
    {
        private bool _preparedUnsetPins;

        public Dictionary<string, PropertyInfo> InputPinsProperties { get; }
        public Dictionary<string, PropertyInfo> OutputPinsProperties { get; }
        public Dictionary<string, IDataPin> Input { get; } = new Dictionary<string, IDataPin>();
        public Dictionary<string, IDataPin> Output { get; } = new Dictionary<string, IDataPin>();
        public string NextKey { get; set; }
        public bool Executed { get; protected set; }

        [NodeInputPin(ValueType = typeof(bool), DefaultValue = false)]
        public IDataPin Bypass
        {
            get
            {
                return GetInput("Bypass");
            }
            set
            {
                Input["Bypass"] = value;
            }
        }

        public NodeBase()
        {
            var curType = GetType();
            var inputProperties = curType.GetProperties().Where(prop => prop.IsDefined(typeof(NodeInputPinAttribute), false)).ToList();
            InputPinsProperties = inputProperties.ToDictionary(x => x.Name, x => x);
            var outputProperties = curType.GetProperties().Where(prop => prop.IsDefined(typeof(NodeOutputPinAttribute), false)).ToList();
            OutputPinsProperties = outputProperties.ToDictionary(x => x.Name, x => x);
        }

        public void PrepareInputDataPin(
            string key,
            Type valueType,
            bool validate = true)
        {
            if(validate && !InputPinsProperties.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Input pin with the key name '{key}' not found on node type {GetType().Name}.");
            }

            if (!Input.ContainsKey(key))
            {
                Input.Add(key, new DataPin(
                    key,
                    null,
                    valueType));
            }
        }

        public void PrepareOutputDataPin(
            string key,
            Type valueType,
            bool validate = true)
        {
            if (validate && !OutputPinsProperties.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Output pin with the key name '{key}' not found on node type {GetType().Name}.");
            }

            if (!Output.ContainsKey(key))
            {
                Output.Add(key, new DataPin(
                    key,
                    null,
                    valueType));
            }
        }

        private void PrepareUnsetPins()
        {
            if (_preparedUnsetPins) return;

            var unsetInput = InputPinsProperties.Where(x => !Input.ContainsKey(x.Key)).ToList();
            foreach (var curUnsetInput in unsetInput)
            {
                var attribute = (NodeInputPinAttribute)Attribute.GetCustomAttribute(curUnsetInput.Value, typeof(NodeInputPinAttribute));
                if (attribute.ValueType != null)
                {
                    Input[curUnsetInput.Key] = new DataPin(
                        curUnsetInput.Key,
                        attribute.DefaultValue,
                        attribute.ValueType);
                }
            }

            var unsetOutput = OutputPinsProperties.Where(x => !Output.ContainsKey(x.Key)).ToList();
            foreach (var curUnsetOutput in unsetOutput)
            {
                var attribute = (NodeOutputPinAttribute)Attribute.GetCustomAttribute(curUnsetOutput.Value, typeof(NodeOutputPinAttribute));
                if (attribute.ValueType != null)
                {
                    Output[curUnsetOutput.Key] = new DataPin(
                        curUnsetOutput.Key,
                        attribute.DefaultValue,
                        attribute.ValueType);
                }
            }

            _preparedUnsetPins = true;
        }

        public async Task ExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            PrepareUnsetPins();
            graph.BeforeExecute(this);
            if(!Bypass.GetValue<bool>())
            {
                await DoExecuteAsync(
                    graph,
                    cancellationToken).ConfigureAwait(false);
            }
            await ExecuteNextAsync(
                graph,
                cancellationToken).ConfigureAwait(false);
        }

        public async Task ExecuteNextAsync(
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
                await nextNode.ExecuteAsync(
                    graph,
                    cancellationToken).ConfigureAwait(false);
            }
        }

        protected virtual Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public IDataPin GetInput(string key)
        {
            PrepareInputDataPin(
                key,
                typeof(object));    //!!! We need to ditch this
            return Input[key];
        }

        public IDataPin GetOutput(string key)
        {
            PrepareOutputDataPin(
                key,
                typeof(object));    //!!! We need to ditch this
            return Output[key];
        }
    }
}