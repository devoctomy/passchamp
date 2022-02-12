using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using System.Diagnostics.CodeAnalysis;

namespace devoctomy.Passchamp.SignTool.Services
{
    [ExcludeFromCodeCoverage]
    public class SignOptions : PreOptions
    {
        [CommandLineParserOption(Required = true, ShortName = "p", LongName = "privatekey")]
        public string KeyFile { get; set; }

        [CommandLineParserOption(Required = true, ShortName = "i", LongName = "input")]
        public string Input { get; set; }

        [CommandLineParserOption(Required = false, ShortName = "o", LongName = "output", DefaultValue = "output.json")]
        public string Output { get; set; }
    }
}
