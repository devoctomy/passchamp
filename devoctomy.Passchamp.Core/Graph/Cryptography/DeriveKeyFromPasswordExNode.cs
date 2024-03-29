﻿using devoctomy.Passchamp.Core.Cryptography;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography;

public class DeriveKeyFromPasswordExNode : NodeBase
{
    private readonly ISecureStringUnpacker _secureStringUnpacker;

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

    [NodeInputPin(ValueType = typeof(int), DefaultValue = 0)]
    public IDataPin<int> KeyLength
    {
        get
        {
            return GetInput<int>("KeyLength");
        }
        set
        {
            Input["KeyLength"] = value;
        }
    }

    [NodeInputPin(ValueType = typeof(int), DefaultValue = 1)]
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

    [NodeOutputPin(ValueType = typeof(byte[]))]
    public IDataPin<byte[]> Key
    {
        get
        {
            return GetOutput<byte[]>("Key");
        }
    }

    public DeriveKeyFromPasswordExNode(ISecureStringUnpacker secureStringUnpacker)
    {
        _secureStringUnpacker = secureStringUnpacker;
    }

    protected override Task DoExecuteAsync(
        IGraph graph,
        CancellationToken cancellationToken)
    {
        void callback(byte[] buffer)
        {
            using var rfc2898 = new System.Security.Cryptography.Rfc2898DeriveBytes(
                buffer,
                Salt.Value,
                IterationCount.Value,
                System.Security.Cryptography.HashAlgorithmName.SHA256);
            Key.Value = rfc2898.GetBytes(KeyLength.Value);
        }
        _secureStringUnpacker.Unpack(SecurePassword.Value, callback);
        return Task.CompletedTask;
    }
}
