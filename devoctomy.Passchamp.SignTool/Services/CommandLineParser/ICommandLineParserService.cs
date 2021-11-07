using System;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public interface ICommandLineParserService
    {
        bool TryParseArgumentsAsOptions<T>(string argumentString, out ParseResults options);
        bool TryParseArgumentsAsOptions(Type optionsType, string argumentString, out ParseResults options);
    }
}
