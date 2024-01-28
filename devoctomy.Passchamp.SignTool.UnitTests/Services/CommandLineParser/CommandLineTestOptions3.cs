using devoctomy.Passchamp.SignTool.Services.CommandLineParser;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services.CommandLineParser;

public class CommandLineTestOptions3
{
    [CommandLineParserOption(
        LongName = "enum",
        ShortName = "e",
        Required = false,
        IsDefault = true,
        DisplayName = "Enum")]
    public TestEnum EnumValue { get; set; }
}
