using System;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public class ParseResults
    {
        public object Options { get; set; }
        public Exception Exception { get; set; }
    
        public T OptionsAs<T>()
        {
            return (T)Options;
        }
    }
}
