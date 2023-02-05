using System;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser;

public interface IHelpMessageFormatter
{
    string Format<T>();
    string Format(Type optionsType);
}
