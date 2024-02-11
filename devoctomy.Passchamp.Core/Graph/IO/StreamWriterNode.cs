using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.IO;

public class StreamWriterNode : NodeBase
{
    [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
    public IDataPin<byte[]> InputData
    {
        get
        {
            return GetInput<byte[]>("InputData");
        }
        set
        {
            Input["InputData"] = value;
        }
    }

    [NodeInputPin(ValueType = typeof(Stream), DefaultValue = null)]
    public IDataPin<Stream> Stream
    {
        get
        {
            return GetInput<Stream>("Stream");
        }
        set
        {
            Input["Stream"] = value;
        }
    }

    protected override async Task DoExecuteAsync(
        IGraph graph,
        CancellationToken cancellationToken)
    {
        var inputData = InputData.Value;

        OutputMessage($"Writing {inputData.Length} bytes to stream.");
        await this.Stream.Value.WriteAsync(
            inputData,
            cancellationToken);
    }
}
