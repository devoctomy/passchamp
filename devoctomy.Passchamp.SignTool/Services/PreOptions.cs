using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using devoctomy.Passchamp.SignTool.Services.Enums;
using System.Diagnostics.CodeAnalysis;

namespace devoctomy.Passchamp.SignTool.Services;

[ExcludeFromCodeCoverage]
public class PreOptions
{
    [CommandLineParserOption(
        Required = true,
        ShortName = "c",
        LongName = "command",
        IsDefault = true,
        DisplayName = "Command",
        HelpText = "Command to perform")]
    public Command Command { get; set; }
}
