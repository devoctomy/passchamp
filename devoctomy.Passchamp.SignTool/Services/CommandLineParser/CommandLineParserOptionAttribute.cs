using System;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandLineParserOptionAttribute : Attribute
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public object DefaultValue { get; set; } 
        public bool Required { get; set; }
        public bool IsDefault { get; set; }
        public string DisplayName { get; set; }
        public string HelpText { get; set; }
    }
}
