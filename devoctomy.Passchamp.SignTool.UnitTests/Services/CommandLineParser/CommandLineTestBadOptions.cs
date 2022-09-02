using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using System;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services.CommandLineParser;

public class CommandLineTestBadOptions
{
    [CommandLineParserOption(LongName = "Unsupported", ShortName = "u", Required = true, IsDefault = true)]
    public Guid UnsupportedValue { get; set; }
}