namespace devoctomy.Passchamp.SignTool.Services
{
    public interface ICommandLineParserService
    {
        T ParseArgumentsAsOptions<T>(string argumentString);
    }
}
