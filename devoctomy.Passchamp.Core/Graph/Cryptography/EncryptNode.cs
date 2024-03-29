﻿using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography;

public class EncryptNode : NodeBase
{
    [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
    public IDataPin<byte[]> PlainTextBytes
    {
        get
        {
            return GetInput<byte[]>("PlainTextBytes");
        }
        set
        {
            Input["PlainTextBytes"] = value;
        }
    }

    [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
    public IDataPin<byte[]> Iv
    {
        get
        {
            return GetInput<byte[]>("Iv");
        }
        set
        {
            Input["Iv"] = value;
        }
    }

    [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
    public IDataPin<byte[]> Key
    {
        get
        {
            return GetInput<byte[]>("Key");
        }
        set
        {
            Input["Key"] = value;
        }
    }

    [NodeOutputPin(ValueType = typeof(byte[]))]
    public IDataPin<byte[]> EncryptedBytes
    {
        get
        {
            return GetOutput<byte[]>("EncryptedBytes");
        }
    }

    protected override async Task DoExecuteAsync(
        IGraph graph,
        CancellationToken cancellationToken)
    {
        using var crypto = Aes.Create();
        var encryptStream = crypto.CreateEncryptor(
                Key.Value,
                Iv.Value);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(
            memoryStream,
            encryptStream,
            CryptoStreamMode.Write);
        await cryptoStream.WriteAsync(
            PlainTextBytes.Value,
            cancellationToken).ConfigureAwait(false);
        await cryptoStream.FlushFinalBlockAsync(cancellationToken).ConfigureAwait(false);

        EncryptedBytes.Value = memoryStream.ToArray();
    }
}
