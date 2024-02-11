using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.IO;

public class StreamReaderNode : NodeBase
{
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

    [NodeOutputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
    public IDataPin<byte[]> Bytes
    {
        get
        {
            return GetOutput<byte[]>("Bytes");
        }
    }

    protected override async Task DoExecuteAsync(
        IGraph graph,
        CancellationToken cancellationToken)
    {
        using (var memoryStream = new MemoryStream())
        {
            byte[] buffer = new byte[81920];
            int bytesRead;

            while ((bytesRead = await Stream.Value.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                memoryStream.Write(buffer, 0, bytesRead);
            }

            Bytes.Value = memoryStream.ToArray();
        }
    }
}
