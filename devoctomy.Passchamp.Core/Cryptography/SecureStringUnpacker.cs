using System;
using System.Runtime.InteropServices;
using System.Security;

namespace devoctomy.Passchamp.Core.Cryptography;

public class SecureStringUnpacker : ISecureStringUnpacker
{
    public void Unpack(
        SecureString value,
        Action<byte[]> callback)
    {
        IntPtr ptr = Marshal.SecureStringToBSTR(value);
        try
        {
            byte[] passwordByteArray = null;
            int length = Marshal.ReadInt32(ptr, -4);
            passwordByteArray = new byte[length];
            GCHandle handle = GCHandle.Alloc(passwordByteArray, GCHandleType.Pinned);
            try
            {
                for (int i = 0; i < length; i++)
                {
                    passwordByteArray[i] = Marshal.ReadByte(ptr, i);
                }

                callback(passwordByteArray);
            }
            finally
            {
                Array.Clear(passwordByteArray, 0, passwordByteArray.Length);
                handle.Free();
            }
        }
        finally
        {
            Marshal.ZeroFreeBSTR(ptr);
        }
    }
}
