﻿using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Text;

public class UnicodeEncoderNode : NodeBase
{
    [NodeInputPin(ValueType = typeof(string), DefaultValue = "")]
    public IDataPin<string> PlainText
    {
        get
        {
            return GetInput<string>("PlainText");
        }
        set
        {
            Input["PlainText"] = value;
        }
    }

    [NodeOutputPin(ValueType = typeof(byte[]))]
    public IDataPin<byte[]> EncodedBytes
    {
        get
        {
            return GetOutput<byte[]>("EncodedBytes");
        }
    }

    protected override Task DoExecuteAsync(
        IGraph graph,
        CancellationToken cancellationToken)
    {
        EncodedBytes.Value = System.Text.Encoding.Unicode.GetBytes(PlainText.Value);
        return Task.CompletedTask;
    }
}
