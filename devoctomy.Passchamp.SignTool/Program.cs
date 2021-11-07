using devoctomy.Passchamp.SignTool.Services;
using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        static async Task<int> Main(string[] args)
        {
            var curExePath = Assembly.GetEntryAssembly().Location;
            var arguments = Environment.CommandLine.Replace(curExePath, string.Empty).Trim();
            var commandLineParser = CommandLineParserService.CreateDefaultInstance();
            if(commandLineParser.TryParseArgumentsAsOptions<PreOptions>(arguments, out var preOptions))
            {
                switch (preOptions.OptionsAs<PreOptions>().Mode.ToLower())
                {
                    case "generate":
                        {
                            if(commandLineParser.TryParseArgumentsAsOptions<GenerateOptions>(arguments, out var generateOptions))
                            {
                                return await Generate(generateOptions.OptionsAs<GenerateOptions>());
                            }
                            else
                            {
                                Console.WriteLine($"{generateOptions.Exception.Message}");
                            }
                            break;
                        }

                    case "sign":
                        {
                            throw new NotImplementedException();
                        }

                    case "verify":
                        {
                            throw new NotImplementedException();
                        }

                    default:
                        {
                            Console.WriteLine($"Unknown mode '{preOptions.OptionsAs<PreOptions>().Mode}'.");
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
