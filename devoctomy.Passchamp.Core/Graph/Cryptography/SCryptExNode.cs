using devoctomy.Passchamp.Core.Cryptography;
using System;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography;

public class SCryptExNode : NodeBase
{
    private readonly ISecureStringUnpacker _secureStringUnpacker;

    [NodeInputPin(ValueType = typeof(int))]
    public IDataPin<int> IterationCount
    {
        get
        {
            return GetInput<int>("IterationCount");
        }
        set
        {
            Input["IterationCount"] = value;
        }
    }

    [NodeInputPin(ValueType = typeof(int))]
    public IDataPin<int> BlockSize
    {
        get
        {
            return GetInput<int>("BlockSize");
        }
        set
        {
            Input["BlockSize"] = value;
        }
    }

    [NodeInputPin(ValueType = typeof(int))]
    public IDataPin<int> ThreadCount
    {
        get
        {
            return GetInput<int>("ThreadCount");
        }
        set
        {
            Input["ThreadCount"] = value;
        }
    }

    [NodeInputPin(ValueType = typeof(SecureString), DefaultValue = null)]
    public IDataPin<SecureString> SecurePassword
    {
        get
        {
            return GetInput<SecureString>("SecurePassword");
        }
        set
        {
            Input["SecurePassword"] = value;
        }
    }

    [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
    public IDataPin<byte[]> Salt
    {
        get
        {
            return GetInput<byte[]>("Salt");
        }
        set
        {
            Input["Salt"] = value;
        }
    }

    [NodeOutputPin(ValueType = typeof(byte[]))]
    public IDataPin<byte[]> Key
    {
        get
        {
            return GetOutput<byte[]>("Key");
        }
    }

    public SCryptExNode(ISecureStringUnpacker secureStringUnpacker)
    {
        _secureStringUnpacker = secureStringUnpacker;
    }

    protected override Task DoExecuteAsync(
        IGraph graph,
        CancellationToken cancellationToken)
    {
        void callback(byte[] buffer)
        {
            var scrypt = new SCrypt(
                IterationCount.Value,
                BlockSize.Value,
                ThreadCount.Value);
            Key.Value = scrypt.DeriveBytes(
                buffer,
                Salt.Value);
        }
        _secureStringUnpacker.Unpack(SecurePassword.Value, callback);
        return Task.CompletedTask;
    }
}
