﻿using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using System.Diagnostics.CodeAnalysis;

namespace devoctomy.Passchamp.SignTool
{
    [ExcludeFromCodeCoverage]
    public class PreOptions
    {
        [CommandLineParserOption(Required = true, ShortName = "m", LongName = "mode", IsDefault = true)]
        public string Mode { get; set; }
    }
}