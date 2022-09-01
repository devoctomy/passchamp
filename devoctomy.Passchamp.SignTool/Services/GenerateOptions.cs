using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using System.Diagnostics.CodeAnalysis;

namespace devoctomy.Passchamp.SignTool.Services;

[ExcludeFromCodeCoverage]
public class GenerateOptions : PreOptions
{
    [CommandLineParserOption(Required = false, ShortName = "l", LongName = "length", DefaultValue = 1024)]
    public int KeyLength { get; set; }

    [CommandLineParserOption(Required = false, ShortName = "v", LongName = "verbose", DefaultValue = true)]
    public bool Verbose { get; set; }
}
