using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography;

public class RandomByteArrayGeneratorNode : NodeBase
{
    [NodeInputPin(ValueType = typeof(int), DefaultValue = 0)]
    public IDataPin<int> Length
    {
        get
        {
            return GetInput<int>("Length");
        }
        set
        {
            Input["Length"] = value;
        }
    }

    [NodeOutputPin(ValueType = typeof(byte[]))]
    public IDataPin<byte[]> RandomBytes
    {
        get
        {
            return GetOutput<byte[]>("RandomBytes");
        }
    }

    protected override Task DoExecuteAsync(
        IGraph graph,
        CancellationToken cancellationToken)
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[Length.Value];
        rng.GetBytes(randomBytes);
        RandomBytes.Value = randomBytes;
        return Task.CompletedTask;
    }
}
