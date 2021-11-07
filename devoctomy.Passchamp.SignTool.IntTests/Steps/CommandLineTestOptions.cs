using devoctomy.Passchamp.SignTool.Services.CommandLineParser;

namespace devoctomy.Passchamp.SignTool.IntTests.Steps
{
    public class CommandLineTestOptions
    {
        [CommandLineParserOption(LongName = "string", ShortName = "s", Required = true, IsDefault = true)]
        public string StringValue { get; set; }

        [CommandLineParserOption(LongName = "bool", ShortName = "b", Required = true)]
        public bool BoolValue { get; set; }

        [CommandLineParserOption(LongName = "int", ShortName = "i", Required = true)]
        public int IntValue { get; set; }

        [CommandLineParserOption(LongName = "float", ShortName = "f", Required = true)]
        public float FloatValue { get; set; }

        [CommandLineParserOption(LongName = "optstring", ShortName = "o", Required = false, DefaultValue = "Hello World")]
        public string OptionalStringValue { get; set; }
    }
}
