namespace devoctomy.Passchamp.Core.Graph.Console
{
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
