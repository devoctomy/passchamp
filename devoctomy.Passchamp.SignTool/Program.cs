using CommandLine;
using System;

namespace devoctomy.Passchamp.SignTool
{
    public class Program
    {
        public class Options
        {
            [Option('i', "input", Required = false, HelpText = "Input json file to sign.")]
            public string Input { get; set; }
            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }
        }


        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       
                   });
        }
    }
}
