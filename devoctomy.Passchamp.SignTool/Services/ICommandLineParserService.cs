namespace devoctomy.Passchamp.SignTool.Services
{
    public interface ICommandLineParserService<T>
    {
        T ParseArgumentsAsOptions(string argumentString);
    }
}
