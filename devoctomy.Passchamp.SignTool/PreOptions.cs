using devoctomy.Passchamp.SignTool.Services.CommandLineParser;

namespace devoctomy.Passchamp.SignTool
{
    public class PreOptions
    {
        [CommandLineParserOption(Required = true, ShortName = "m", LongName = "mode", IsDefault = true)]
        public string Mode { get; set; }
    }
}
