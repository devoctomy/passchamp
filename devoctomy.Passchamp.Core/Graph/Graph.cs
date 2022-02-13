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
        //private readonly List<string> _executionOrder = new();
        private readonly Dictionary<string, IPin> _inputPins;
        private readonly Dictionary<string, IPin> _outputPins;
        private readonly IEnumerable<IGraphPinPrepFunction> _pinPrepFunctions;
        private readonly IEnumerable<IGraphPinOutputFunction> _pinOutputFunctions;

        public GraphSettings Settings { get; set; }
        public IGraph.GraphOutputMessageDelegate OutputMessage { get; set; }
        public IReadOnlyDictionary<string, IPin> InputPins => _inputPins;
        public IReadOnlyDictionary<string, IPin> OutputPins => _outputPins;
        //public IReadOnlyList<string> ExecutionOrder => _executionOrder;
        public IReadOnlyDictionary<string, INode> Nodes { get; }
        public IReadOnlyDictionary<INode, string> NodeKeys => _nodeKeys;
        public Dictionary<string, object> ExtendedParams { get; } = new Dictionary<string, object>();
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
            GraphSettings settings,
            Dictionary<string, IPin> inputPins,
            Dictionary<string, IPin> outputPins,
            Dictionary<string, INode> nodes,
            string startKey,
            IGraph.GraphOutputMessageDelegate outputMessage,
            IEnumerable<IGraphPinPrepFunction> pinPrepFunctions,
            IEnumerable<IGraphPinOutputFunction> pinOutputFunctions)
        {
            Settings = settings;
            OutputMessage = outputMessage;
            _inputPins = inputPins;
            _outputPins = outputPins;
            Nodes = nodes;
            //_nodeKeys = Nodes?.ToList().ToDictionary(
            //    x => x.Value,
            //    x => x.Key);    // !!! BUG, We don't support more than one node of the same type!
            if(Nodes != null)
            {
                foreach (var curNode in Nodes)
                {
                    DoOutputMessage($"Attaching node {curNode.Key} of type '{curNode.Value.GetType().Name}'");
                    curNode.Value.AttachGraph(this);
                }
            }

            StartKey = startKey;
            _pinPrepFunctions = pinPrepFunctions ?? new List<IGraphPinPrepFunction>();
            _pinOutputFunctions = pinOutputFunctions ?? new List<IGraphPinOutputFunction>();
        }

        private void PreparePins()
        {
            DoOutputMessage("Preparing all node pins...");
            foreach (var curNodeKey in Nodes.Keys)
            {
                var curNode = Nodes[curNodeKey];
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
                            curNode.Input[curInputPinKey] = InputPins[path[1]];
                        }
                        else
                        {
                            var pinPrepFunction = _pinPrepFunctions.SingleOrDefault(x => x.IsApplicable(path[0]));
                            if(pinPrepFunction != null)
                            {
                                var node = pinPrepFunction.Execute(
                                    curNodeKey,
                                    intermediateValue.Value,
                                    InputPins,
                                    Nodes);
                                if(node != null)
                                {
                                    curNode.Input[curInputPinKey] = node;
                                }
                            }
                            else
                            {
                                var mapFromNode = Nodes[path[0]];
                                var outPinPropInfo = mapFromNode.OutputPinsProperties[path[1]];
                                var attribute = (NodeOutputPinAttribute)Attribute.GetCustomAttribute(outPinPropInfo, typeof(NodeOutputPinAttribute));
                                mapFromNode.PrepareOutputDataPin(path[1], attribute.ValueType, true);
                                var nodeOutPin = mapFromNode.Output[path[1]];
                                curNode.Input[curInputPinKey] = nodeOutPin;
                            }
                        }
                    }
                }
            }
            DoOutputMessage("Pins prepared");
        }

        private void ProcessOutputPins()
        {
            if(OutputPins != null && OutputPins.Any())
            {
                DoOutputMessage("Processing all output pins...");
                foreach (var curOutputPinKey in OutputPins.Keys.ToList())
                {
                    var outputPin = OutputPins[curOutputPinKey];
                    var path = outputPin.ObjectValue.ToString().Split('.');
                    if (path[0] == "Pins")
                    {
                        _outputPins[curOutputPinKey] = InputPins[path[1]];
                    }
                    else
                    {
                        var pinPrepFunction = _pinOutputFunctions.SingleOrDefault(x => x.IsApplicable(path[0]));
                        if (pinPrepFunction != null)
                        {
                            var result = pinPrepFunction.Execute(
                                outputPin.ObjectValue.ToString(),
                                Nodes);
                            _outputPins[curOutputPinKey] = result;
                        }
                        else
                        {
                            if(Nodes.ContainsKey(path[0]))
                            {
                                var node = Nodes[path[0]];
                                var nodeOutputPin = node.Output[path[1]];
                                _outputPins[curOutputPinKey] = nodeOutputPin;
                            }
                        }
                    }
                }
                DoOutputMessage("Pins processed");
            }
        }

        public T GetNode<T>(string key) where T : INode
        {
            return Nodes.ContainsKey(key) ? (T)Nodes[key] : default;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            DoOutputMessage($"Executing graph...");
            //_executionOrder.Clear();
            PreparePins();
            var startNode = GetNode<INode>(StartKey);
            await startNode.ExecuteAsync(cancellationToken).ConfigureAwait(false);
            ProcessOutputPins();
        }

        public void BeforeExecute(INode node)
        {
            DoOutputMessage($"Before execute node...");
            //if (_nodeKeys.TryGetValue(node, out string key))
            //{
            //    _executionOrder.Add(key);
            //}
        }

        private void DoOutputMessage(string message)
        {
            OutputMessage?.Invoke(
                null,
                message);
        }
    }
}