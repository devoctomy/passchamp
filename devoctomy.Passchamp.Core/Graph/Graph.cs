using devoctomy.Passchamp.Core.Graph.Services;
using System;
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
        private readonly Dictionary<string, IPin> _pins;

        public IGraph.GraphOutputMessageDelegate OutputMessage { get; set; }
        public Dictionary<string, IPin> Pins => _pins;
        public IReadOnlyList<string> ExecutionOrder => _executionOrder;
        public Dictionary<string, INode> Nodes { get; }
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
            Dictionary<string, IPin> pins,
            Dictionary<string, INode> nodes,
            string startKey,
            IGraph.GraphOutputMessageDelegate outputMessage)
        {
            OutputMessage = outputMessage;
            _pins = pins;
            Nodes = nodes;
            _nodeKeys = Nodes.ToList().ToDictionary(
                x => x.Value,
                x => x.Key);
            foreach(var curNode in Nodes)
            {
                DoOutputMessage($"Attaching node {curNode.Key} of type '{curNode.Value.GetType().Name}'");
                curNode.Value.AttachGraph(this);
            }
            StartKey = startKey;
        }

        private void PreparePins()
        {
            DoOutputMessage("Preparing pins...");
            foreach (var curNode in Nodes.Values)
            {
                foreach(var curInputPinKey in curNode.Input.Keys.ToArray())
                {
                    var curInputPinValue = curNode.Input[curInputPinKey];
                    if(curInputPinValue != null &&
                        curInputPinValue.ObjectValue != null &&
                        curInputPinValue.ObjectValue.GetType() == typeof(DataPinIntermediateValue))
                    {
                        var intermediateValue = curInputPinValue.ObjectValue as DataPinIntermediateValue;
                        var path = intermediateValue.Value.Split('.');
                        if(path[0] == "Pins")
                        {
                            curNode.Input[curInputPinKey] = Pins[path[1]];
                        }
                        else
                        {
                            var mapFromNode = Nodes[path[0]];
                            var outPinPropInfo = mapFromNode.OutputPinsProperties[path[1]];
                            var attribute = (NodeOutputPinAttribute)Attribute.GetCustomAttribute(outPinPropInfo, typeof(NodeOutputPinAttribute));
                            mapFromNode.PrepareOutputDataPin(path[1], attribute.ValueType);
                            var nodeOutPin = mapFromNode.Output[path[1]];
                            curNode.Input[curInputPinKey] = nodeOutPin;
                        }
                    }
                }
            }
            DoOutputMessage("Pins prepared");
        }

        public T GetNode<T>(string key) where T : INode
        {
            return Nodes.ContainsKey(key) ? (T)Nodes[key] : default;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            DoOutputMessage($"Executing graph...");
            _executionOrder.Clear();
            PreparePins();
            var startNode = GetNode<INode>(StartKey);
            await startNode.ExecuteAsync(cancellationToken).ConfigureAwait(false);
        }

        public void BeforeExecute(INode node)
        {
            DoOutputMessage($"Before execute node...");
            if (_nodeKeys.TryGetValue(node, out string key))
            {
                _executionOrder.Add(key);
            }
        }

        private void DoOutputMessage(string message)
        {
            if (OutputMessage != null)
            {
                OutputMessage(
                    null,
                    message);
            }
        }
    }
}