using devoctomy.Passchamp.SignTool.Services.CommandLineParser;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services.CommandLineParser;

public class CommandLineTestOptions
{
    [CommandLineParserOption(
        LongName = "string",
        ShortName = "s",
        Required = true,
        IsDefault = true,
        DisplayName = "String",
        HelpText = "Some required string value.")]
    public string StringValue { get; set; }

    [CommandLineParserOption(
        LongName = "bool",
        ShortName = "b",
        Required = true,
        DisplayName = "Boolean",
        HelpText = "Some required bool value.")]
    public bool BoolValue { get; set; }

    [CommandLineParserOption(
        LongName = "int",
        ShortName = "i",
        Required = true,
        DisplayName = "Integer",
        HelpText = "Some required int value")]
    public int IntValue { get; set; }

    [CommandLineParserOption(
        LongName = "float",
        ShortName = "f",
        Required = true,
        DisplayName = "Floating Point",
        HelpText = "Some required float value")]
    public float FloatValue { get; set; }

    [CommandLineParserOption(
        LongName = "optstring",
        ShortName = "o",
        Required = false,
        DefaultValue = "Hello World",
        DisplayName = "Optional String",
        HelpText = "Some optional string value")]
    public string OptionalStringValue { get; set; }

    [CommandLineParserOption(
        LongName = "enum",
        ShortName = "e",
        Required = false,
        DefaultValue = "None",
        DisplayName = "Optional Enum",
        HelpText = "Some optional enum value")]
    public TestEnum OptionalEnumValue { get; set; }
}