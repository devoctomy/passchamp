using System;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public class ParseResults<T>
    {
        public T Options { get; set; }
        public Exception Exception { get; set; }
    }
}
