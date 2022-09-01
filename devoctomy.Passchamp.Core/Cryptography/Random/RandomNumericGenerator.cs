using System;
using System.Security.Cryptography;

namespace devoctomy.Passchamp.Core.Cryptography.Random;

public class RandomNumericGenerator : IRandomNumericGenerator
{
    public double GenerateDouble()
    {
        var bytes = RandomNumberGenerator.GetBytes(8);
        var unscaled = BitConverter.ToUInt64(bytes, 0);
        unscaled &= ((1UL << 53) - 1);
        var random = (double)unscaled / (double)(1UL << 53);
        return (random);
    }

    public int GenerateInt(
        int min,
        int max)
    {
        return RandomNumberGenerator.GetInt32(min, max);
    }
}
