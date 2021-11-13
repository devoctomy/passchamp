using devoctomy.Passchamp.SignTool.Services;
using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using System;
using System.IO;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool
{
    public class SignToolProgram : IProgram
    {
        private readonly ICommandLineArgumentService _commandLineArgumentService;
        private readonly ICommandLineParserService _commandLineParserService;

        public SignToolProgram(
            ICommandLineArgumentService commandLineArgumentService,
            ICommandLineParserService commandLineParserService)
        {
            _commandLineArgumentService = commandLineArgumentService;
            _commandLineParserService = commandLineParserService;
        }

        public async Task Run()
        {
            var arguments = _commandLineArgumentService.GetArguments(Environment.CommandLine);
            if (_commandLineParserService.TryParseArgumentsAsOptions<PreOptions>(arguments, out var preOptions))
            {
                switch (preOptions.OptionsAs<PreOptions>().Mode.ToLower())
                {
                    case "generate":
                        {
                            if (_commandLineParserService.TryParseArgumentsAsOptions<GenerateOptions>(arguments, out var generateOptions))
                            {
                                await Generate(generateOptions.OptionsAs<GenerateOptions>());
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
