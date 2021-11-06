using devoctomy.Passchamp.SignTool.Services;
using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool
{
    public class Program
    {
        public class PreOptions
        {
            [CommandLineParserOption(Required = true, ShortName = "m", LongName = "mode", IsDefault = true)]
            public string Mode { get; set; }
        }

        public class GenerateOptions : PreOptions
        {
            [CommandLineParserOption(Required = false, ShortName = "l", LongName = "length", DefaultValue = 1024)]
            public int KeyLength { get; set; }

            [CommandLineParserOption(Required = false, ShortName = "v", LongName = "verbose", DefaultValue = false)]
            public bool Verbose { get; set; }
        }

        static async Task<int> Main(string[] args)
        {
            var curExePath = Assembly.GetEntryAssembly().Location;
            var arguments = Environment.CommandLine.Replace(curExePath, string.Empty).Trim();
            var commandLineParser = CommandLineParserService.CreateDefaultInstance();
            if(commandLineParser.TryParseArgumentsAsOptions<PreOptions>(arguments, out var preOptions))
            {
                switch (preOptions.Options.Mode.ToLower())
                {
                    case "generate":
                        {
                            if(commandLineParser.TryParseArgumentsAsOptions<GenerateOptions>(arguments, out var generateOptions))
                            {
                                return await Generate(generateOptions.Options);
                            }
                            else
                            {
                                Console.WriteLine($"{generateOptions.Exception.Message}");
                            }
                            break;
                        }

                    default:
                        {
                            Console.WriteLine($"Unknown mode '{preOptions.Options.Mode}'.");
                            break;
                        }
                }
            }
            else
            {
                Console.WriteLine($"{preOptions.Exception.Message}");
            }

            return -1;
        }

        private static async Task<int> Generate(GenerateOptions options)
        {
            if (options.Verbose)
            {
                Console.WriteLine($"Attempting to generate {options.KeyLength} bit key pair.");
            }

            var keyGenerator = new RsaKeyGeneratorService();
            keyGenerator.Generate(
                options.KeyLength,
                out var privateKey,
                out var publicKey);

            if (options.Verbose)
            {
                Console.WriteLine($"Writing private key.");
            }

            await File.WriteAllTextAsync(
                "privatekey.json",
                privateKey);

            if (options.Verbose)
            {
                Console.WriteLine($"Writing public key.");
            }

            await File.WriteAllTextAsync(
                "publickey.json",
                publicKey);

            return 0;
        }
    }
}
