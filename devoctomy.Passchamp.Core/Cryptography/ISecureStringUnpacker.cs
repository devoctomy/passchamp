using System;
using System.Security;

namespace devoctomy.Passchamp.Core.Cryptography
{
    public interface ISecureStringUnpacker
    {
        void Unpack(SecureString value, Action<byte[]> callback);
    }
}
