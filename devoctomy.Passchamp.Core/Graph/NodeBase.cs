using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph;

public class NodeBase : INode
{
    private IGraph _graph;
    private bool _preparedUnsetPins;

    public string NodeKey { get; set; }
    public Dictionary<string, PropertyInfo> InputPinsProperties { get; }
    public Dictionary<string, PropertyInfo> OutputPinsProperties { get; }
    public Dictionary<string, IPin> Input { get; } = new Dictionary<string, IPin>();
    public Dictionary<string, IPin> Output { get; } = new Dictionary<string, IPin>();
    public string NextKey { get; set; }
    public bool Executed { get; protected set; }

    [NodeInputPin(ValueType = typeof(bool), DefaultValue = false)]
    public IDataPin<bool> Bypass
    {
        get
        {
            return GetInput<bool>("Bypass");
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
        bool validate)
    {
        if(validate && !InputPinsProperties.ContainsKey(key))
        {
            throw new KeyNotFoundException($"Input pin with the key name '{key}' not found on node type {GetType().Name}.");
        }

        if (!Input.ContainsKey(key))
        {
            OutputMessage($"Creating default input pin for '{key}'.");
            Input.Add(key, DataPinFactory.Instance.Create(
                key,
                null,
                valueType));
        }
    }

    public void PrepareOutputDataPin(
        string key,
        Type valueType,
        bool validate)
    {
        if (validate && !OutputPinsProperties.ContainsKey(key))
        {
            throw new KeyNotFoundException($"Output pin with the key name '{key}' not found on node type {GetType().Name}.");
        }

        if (!Output.ContainsKey(key))
        {
            OutputMessage($"Creating default output pin for '{key}'.");
            Output.Add(key, DataPinFactory.Instance.Create(
                key,
                null,
                valueType));
        }
    }

    private void PrepareUnsetPins()
    {
        if (_preparedUnsetPins)
        {
            return;
        }

        OutputMessage("Preparing unset input pins...");
        var unsetInput = InputPinsProperties.Where(x => !Input.ContainsKey(x.Key)).ToList();
        foreach (var curUnsetInput in unsetInput)
        {
            var attribute = (NodeInputPinAttribute)Attribute.GetCustomAttribute(curUnsetInput.Value, typeof(NodeInputPinAttribute));
            if (attribute.ValueType != null)
            {
                Input[curUnsetInput.Key] = DataPinFactory.Instance.Create(
                    curUnsetInput.Key,
                    attribute.DefaultValue,
                    attribute.ValueType);
            }
        }
        OutputMessage("Finished preparing unset input pins.");

        OutputMessage("Preparing unset output pins...");
        var unsetOutput = OutputPinsProperties.Where(x => !Output.ContainsKey(x.Key)).ToList();
        foreach (var curUnsetOutput in unsetOutput)
        {
            var attribute = (NodeOutputPinAttribute)Attribute.GetCustomAttribute(curUnsetOutput.Value, typeof(NodeOutputPinAttribute));
            if (attribute.ValueType != null)
            {
                Output[curUnsetOutput.Key] = DataPinFactory.Instance.Create(
                    curUnsetOutput.Key,
                    attribute.DefaultValue,
                    attribute.ValueType);
            }
        }
        OutputMessage("Finished preparing unset output pins.");

        _preparedUnsetPins = true;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if(_graph == null)
        {
            throw new InvalidOperationException("No graph attached to node.");
        }

        PrepareUnsetPins();
        _graph.BeforeExecute(this);
        if(!Bypass.Value)
        {
            OutputMessage("Executing node...");
            await DoExecuteAsync(
                _graph,
                cancellationToken).ConfigureAwait(false);
        }
        await ExecuteNextAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task ExecuteNextAsync(CancellationToken cancellationToken)
    {
        Executed = true;
        if(string.IsNullOrEmpty(NextKey))
        {
            OutputMessage("Nothing to execute next.");
            return;
        }

        OutputMessage($"Getting next node '{NextKey}'...");
        var nextNode = _graph.GetNode<INode>(NextKey);
        if (nextNode != null)
        {
            OutputMessage($"Executing next node '{NextKey}'...");
            await nextNode.ExecuteAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    protected virtual Task DoExecuteAsync(
        IGraph graph,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public IDataPin<T> GetInput<T>(string key)
    {
        PrepareInputDataPin(
            key,
            typeof(T),
            true);    //!!! We need to ditch this
        return (IDataPin<T>)Input[key];
    }

    public IPin GetInput(
        string key,
        Type type)
    {
        PrepareInputDataPin(
            key,
            type,
            true);    //!!! We need to ditch this
        return Input[key];
    }

    public IDataPin<T> GetOutput<T>(string key)
    {
        PrepareOutputDataPin(
            key,
            typeof(T),
            true);    //!!! We need to ditch this
        return (IDataPin<T>)Output[key];
    }

    public IPin GetOutput(
        string key,
        Type type)
    {
        PrepareOutputDataPin(
            key,
            type,
            true);    //!!! We need to ditch this
        return Output[key];
    }

    public void AttachGraph(IGraph graph)
    {
        _graph = graph ?? throw new ArgumentException(
                "Value cannot be null",
                nameof(graph));
    }

    public void OutputMessage(string message)
    {
        if(_graph != null && _graph.OutputMessage != null)
        {
            _graph.OutputMessage(
                this,
                message);
        }
    }
}