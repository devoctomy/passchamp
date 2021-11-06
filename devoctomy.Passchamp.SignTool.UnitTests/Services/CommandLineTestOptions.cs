using devoctomy.Passchamp.SignTool.Services;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services
{
    public class CommandLineTestOptions
    {
        [CommandLineParserOptionAttribute(LongName = "string", ShortName = "s", Required = true)]
        public string StringValue { get; set; }

        [CommandLineParserOptionAttribute(LongName = "bool", ShortName = "b", Required = true)]
        public bool BoolValue { get; set; }

        [CommandLineParserOptionAttribute(LongName = "int", ShortName = "i", Required = true)]
        public int IntValue { get; set; }

        [CommandLineParserOptionAttribute(LongName = "float", ShortName = "f", Required = true)]
        public float FloatValue { get; set; }

        [CommandLineParserOptionAttribute(LongName = "optstring", ShortName = "o", Required = false, DefaultValue = "Hello World")]
        public string OptionalStringValue { get; set; }
    }
}
