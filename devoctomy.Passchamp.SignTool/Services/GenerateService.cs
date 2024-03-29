﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services;

public class GenerateService : IGenerateService
{
    public async Task<int> Generate(GenerateOptions options)
    {
        if (options.Verbose)
        {
            Console.WriteLine($"Attempting to generate {options.KeyLength} bit key pair.");
        }

        var keyGenerator = new RsaKeyGeneratorService();
        var keyPair = keyGenerator.Generate(options.KeyLength);

        if (options.Verbose)
        {
            Console.WriteLine($"Writing private key.");
        }

        await File.WriteAllTextAsync(
            "privatekey.json",
            keyPair.PrivateKey).ConfigureAwait(false);

        if (options.Verbose)
        {
            Console.WriteLine($"Writing public key.");
        }

        await File.WriteAllTextAsync(
            "publickey.json",
            keyPair.PublicKey);

        return 0;
    }
}
