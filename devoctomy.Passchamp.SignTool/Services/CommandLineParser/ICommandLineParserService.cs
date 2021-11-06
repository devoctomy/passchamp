namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public interface ICommandLineParserService
    {
        T ParseArgumentsAsOptions<T>(string argumentString);
    }
}
