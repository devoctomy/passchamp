using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Text;

public class UnicodeDecoderNode : NodeBase
{
    [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
    public IDataPin<byte[]> EncodedBytes
    {
        get
        {
            return GetInput<byte[]>("EncodedBytes");
        }
        set
        {
            Input["EncodedBytes"] = value;
        }
    }

    [NodeOutputPin(ValueType = typeof(string), DefaultValue = "")]
    public IDataPin<string> PlainText
    {
        get
        {
            return GetOutput<string>("PlainText");
        }
    }

    protected override Task DoExecuteAsync(
        IGraph graph,
        CancellationToken cancellationToken)
    {
        PlainText.Value = System.Text.Encoding.Unicode.GetString(EncodedBytes.Value);
        return Task.CompletedTask;
    }
}
