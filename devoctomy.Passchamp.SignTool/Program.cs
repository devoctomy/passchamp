using devoctomy.Passchamp.SignTool.Services;
using System;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool
{
    public class Program
    {
        public class Options
        {
            [CommandLineParserOption(Required = true, IsDefaultOption = true, ShortName = "m", LongName = "mode")]
            public string Mode { get; set; }
        }

        static int Main(string[] args)
        {
            throw new NotImplementedException();
            //if (o.Verbose)
            //{
            //    Console.WriteLine($"Attempting to generate {o.KeyLength} bit key pair.");
            //}

            //var keyGenerator = new RsaKeyGeneratorService();
            //keyGenerator.Generate(
            //    o.KeyLength,
            //    out var privateKey,
            //    out var publicKey);

            //if (o.Verbose)
            //{
            //    Console.WriteLine($"Writing private key.");
            //}

            //await File.WriteAllTextAsync(
            //    "privatekey.json",
            //    privateKey);

            //if (o.Verbose)
            //{
            //    Console.WriteLine($"Writing public key.");
            //}

            //await File.WriteAllTextAsync(
            //    "publickey.json",
            //    publicKey);
        }
    }
}
