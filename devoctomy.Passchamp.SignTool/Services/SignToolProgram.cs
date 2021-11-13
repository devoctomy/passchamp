using devoctomy.Passchamp.SignTool.Services;
using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using System;
using System.IO;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services
{
    public class SignToolProgram : IProgram
    {
        private readonly ICommandLineArgumentService _commandLineArgumentService;
        private readonly ICommandLineParserService _commandLineParserService;
        private readonly IGenerateService _generateService;

        public SignToolProgram(
            ICommandLineArgumentService commandLineArgumentService,
            ICommandLineParserService commandLineParserService,
            IGenerateService generateService)
        {
            _commandLineArgumentService = commandLineArgumentService;
            _commandLineParserService = commandLineParserService;
            _generateService = generateService;
        }

        public async Task<int> Run()
        {
            var arguments = _commandLineArgumentService.GetArguments(Environment.CommandLine);
            if (_commandLineParserService.TryParseArgumentsAsOptions(typeof(PreOptions), arguments, out var preOptions))
            {
                switch (preOptions.OptionsAs<PreOptions>().Mode.ToLower())
                {
                    case "generate":
                        {
                            if (_commandLineParserService.TryParseArgumentsAsOptions(
                                typeof(GenerateOptions),
                                arguments,
                                out var generateOptions))
                            {
                                return await _generateService.Generate(generateOptions.OptionsAs<GenerateOptions>());
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
    }
}
