namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public interface ICommandLineParserService
    {
        bool TryParseArgumentsAsOptions<T>(string argumentString, out ParseResults<T> options);
    }
}
