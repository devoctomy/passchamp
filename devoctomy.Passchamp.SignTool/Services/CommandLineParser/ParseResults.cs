using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    [ExcludeFromCodeCoverage]
    public class ParseResults
    {
        public object Options { get; set; }
        public Exception Exception { get; set; }
        public Dictionary<string, string> InvalidOptions { get; } = new Dictionary<string, string>();
    
        public T OptionsAs<T>()
        {
            return (T)Options;
        }
    }
}
