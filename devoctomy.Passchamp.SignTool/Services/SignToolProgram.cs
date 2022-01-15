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
                switch (preOptions.OptionsAs<PreOptions>().Mode)
                {
                    case Mode.Generate:
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

                    case Mode.Sign:
                        {
                            throw new NotImplementedException();
                        }

                    case Mode.Verify:
                        {
                            throw new NotImplementedException();
                        }

                    default:
                        {
                            Console.WriteLine($"Mode {preOptions.OptionsAs<PreOptions>().Mode} not yet implemented.");
                            break;
                        }
                }
            }
            else
            {
                Console.WriteLine($"Display help message...");
            }

            return -1;
        }
    }
}
