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
        private readonly Dictionary<string, IDataPin> _pins;

        public Dictionary<string, IDataPin> Pins => _pins;
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
            Dictionary<string, IDataPin> pins,
            Dictionary<string, INode> nodes,
            string startKey)
        {
            _pins = pins;
            Nodes = nodes;
            _nodeKeys = Nodes.ToList().ToDictionary(
                x => x.Value,
                x => x.Key);
            StartKey = startKey;
        }

        private void PreparePins()
        {
            foreach(var curNode in Nodes.Values)
            {
                foreach(var curInputPinKey in curNode.Input.Keys.ToArray())
                {
                    var curInputPinValue = curNode.Input[curInputPinKey];
                    if(curInputPinValue != null &&
                        curInputPinValue.Value != null &&
                        curInputPinValue.Value.GetType() == typeof(DataPinIntermediateValue))
                    {
                        var intermediateValue = curInputPinValue.Value as DataPinIntermediateValue;
                        var path = intermediateValue.Value.Split('.');
                        if(path[0] == "Pins")
                        {
                            curNode.Input[curInputPinKey] = Pins[path[1]];
                        }
                        else
                        {
                            var mapFromNode = Nodes[path[0]];
                            mapFromNode.PrepareOutputDataPin(path[1]);
                            var nodeOutPin = mapFromNode.Output[path[1]];
                            curNode.Input[curInputPinKey] = nodeOutPin;
                        }
                    }
                }
            }
        }

        public T GetNode<T>(string key) where T : INode
        {
            return Nodes.ContainsKey(key) ? (T)Nodes[key] : default;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _executionOrder.Clear();
            PreparePins();
            var startNode = GetNode<INode>(StartKey);
            await startNode.ExecuteAsync(
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