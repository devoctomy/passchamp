using devoctomy.Passchamp.SignTool.Services;
using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services
{
    public class SignToolProgram : IProgram
    {
        private readonly ICommandLineArgumentService _commandLineArgumentService;
        private readonly ICommandLineParserService _commandLineParserService;
        private readonly IGenerateService _generateService;
        private readonly IHelpMessageFormatter _helpMessageFormatter;

        public SignToolProgram(
            ICommandLineArgumentService commandLineArgumentService,
            ICommandLineParserService commandLineParserService,
            IGenerateService generateService,
            IHelpMessageFormatter helpMessageFormatter)
        {
            _commandLineArgumentService = commandLineArgumentService;
            _commandLineParserService = commandLineParserService;
            _generateService = generateService;
            _helpMessageFormatter = helpMessageFormatter;
        }

        public async Task<int> Run()
        {
            var arguments = _commandLineArgumentService.GetArguments(Environment.CommandLine);
            if (_commandLineParserService.TryParseArgumentsAsOptions(typeof(PreOptions), arguments, out var preOptions))
            {
                switch (preOptions.OptionsAs<PreOptions>().Command)
                {
                    case Command.Generate:
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

                    case Command.Sign:
                        {
                            throw new NotImplementedException();
                        }

                    case Command.Verify:
                        {
                            throw new NotImplementedException();
                        }

                    default:
                        {
                            Console.WriteLine($"Command {preOptions.OptionsAs<PreOptions>().Command} not yet implemented.");
                            break;
                        }
                }
            }
            else
            {
                string strExeFilePath = System.Reflection.Assembly.GetEntryAssembly().Location;
                var helpMessage = _helpMessageFormatter.Format<PreOptions>();
                var message = new StringBuilder();
                if(preOptions.InvalidOptions.ContainsKey("Command"))
                {
                    var expected = Enum.GetNames<Command>().Where(x => x != "None").Select(y => y.ToLower()).ToList();
                    message.AppendLine($"Unknown command '{preOptions.InvalidOptions["Command"]}', expected one of ({string.Join(',', expected)}).");
                }
                else
                {
                    message.AppendLine($"Invalid command line '{Environment.CommandLine}'.");
                }

                message.AppendLine();
                message.AppendLine($"Usage: {new FileInfo(strExeFilePath).Name} [command] [command_options]");
                message.AppendLine();
                message.Append(helpMessage);
                Console.WriteLine(message);
            }

            return -1;
        }
    }
}
