using System;

namespace devoctomy.Passchamp.SignTool.Services
{
    public class CommandLineParserOptionAttribute : Attribute
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public object DefaultValue { get; set; } 
        public bool Required { get; set; }
    }
}
