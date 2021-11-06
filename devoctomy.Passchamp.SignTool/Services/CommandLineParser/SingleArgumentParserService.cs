namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public class SingleArgumentParserService : ISingleArgumentParserService
    {
        public Argument Parse(string argumentString)
        {
            if (string.IsNullOrEmpty(argumentString))
            {
                return new Argument();
            }    

            var parts = argumentString.Split('=');
            return new Argument
            {
                Name = parts[0],
                Value = parts[1].Trim('\"')
            };
        }
    }
}
