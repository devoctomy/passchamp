using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography;

public class DecryptNode : NodeBase
{
    private const string AesAlgorithmName = "AesManaged";

    [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
    public IDataPin<byte[]> Cipher
    {
        get
        {
            return GetInput<byte[]>("Cipher");
        }
        set
        {
            Input["Cipher"] = value;
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
    public IDataPin<byte[]> DecryptedBytes
    {
        get
        {
            return GetOutput<byte[]>("DecryptedBytes");
        }
    }

    protected override async Task DoExecuteAsync(
        IGraph graph,
        CancellationToken cancellationToken)
    {
        using var crypto = Aes.Create(AesAlgorithmName);
        using var decryptStream = crypto.CreateDecryptor(
                Key.Value,
                Iv.Value);
        using var cryptoStream = new CryptoStream(
            new MemoryStream(Cipher.Value),
            decryptStream,
            CryptoStreamMode.Read);
        using var output = new MemoryStream();
        await cryptoStream.CopyToAsync(
            output,
            cancellationToken).ConfigureAwait(false);

        DecryptedBytes.Value = output.ToArray();
    }
}
