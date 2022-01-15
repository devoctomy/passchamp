using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using System.Diagnostics.CodeAnalysis;

namespace devoctomy.Passchamp.SignTool.Services
{
    [ExcludeFromCodeCoverage]
    public class PreOptions
    {
        [CommandLineParserOption(Required = true, ShortName = "m", LongName = "mode", IsDefault = true, HelpText = "Mode of operation")]
        public Mode Mode { get; set; }
    }
}
