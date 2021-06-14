using System.Diagnostics.CodeAnalysis;

namespace devoctomy.Passchamp.Core.Graph.Console
{
    [ExcludeFromCodeCoverage]
    public class SystemConsole : ISystemConsole
    {
        public string ReadLine()
        {
            return System.Console.ReadLine();
        }

        public void WriteLine(string value)
        {
            System.Console.WriteLine(value);
        }
    }
}
