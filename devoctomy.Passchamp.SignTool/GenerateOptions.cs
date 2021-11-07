using devoctomy.Passchamp.SignTool.Services.CommandLineParser;

namespace devoctomy.Passchamp.SignTool
{
    public class GenerateOptions : PreOptions
    {
        [CommandLineParserOption(Required = false, ShortName = "l", LongName = "length", DefaultValue = 1024)]
        public int KeyLength { get; set; }

        [CommandLineParserOption(Required = false, ShortName = "v", LongName = "verbose", DefaultValue = false)]
        public bool Verbose { get; set; }
    }
}
