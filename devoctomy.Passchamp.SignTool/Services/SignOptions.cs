using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using System.Diagnostics.CodeAnalysis;

namespace devoctomy.Passchamp.SignTool.Services;

[ExcludeFromCodeCoverage]
public class VerifyOptions : PreOptions
{
    [CommandLineParserOption(Required = true, ShortName = "p", LongName = "publickey")]
    public string KeyFile { get; set; }

    [CommandLineParserOption(Required = true, ShortName = "i", LongName = "input")]
    public string Input { get; set; }
}
