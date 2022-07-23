using System;
using System.Linq;
using System.Text;

namespace devoctomy.Passchamp.SignTool.Services.CommandLineParser
{
    public class HelpMessageFormatter : IHelpMessageFormatter
    {
        public string Format<T>()
        {
            return Format(typeof(T));
        }

        public string Format(Type optionsType)
        {
            var helpMessage = new StringBuilder();
            var allProperties = optionsType.GetProperties().OrderBy(x => x.Name);

            var requiredOptions = new StringBuilder();
            var optionalOptions = new StringBuilder();
            foreach (var curProperty in allProperties)
            {
                var optionAttribute = (CommandLineParserOptionAttribute)curProperty.GetCustomAttributes(typeof(CommandLineParserOptionAttribute), true).FirstOrDefault();
                if (optionAttribute == null || string.IsNullOrWhiteSpace(optionAttribute.HelpText))
                {
                    continue;
                }

                var option = new StringBuilder();
                option.AppendLine($"\t\tName:\t{optionAttribute.DisplayName}");
                if(!optionAttribute.IsDefault)
                {
                    option.AppendLine($"\t\tShort:\t-{optionAttribute.ShortName}");
                    option.AppendLine($"\t\tLong:\t--{optionAttribute.LongName}");
                }

                option.AppendLine($"\t\t{optionAttribute.HelpText}");
                if (optionAttribute.Required)
                {
                    requiredOptions.AppendLine(option.ToString());
                }
                else
                {
                    optionalOptions.AppendLine(option.ToString());
                }
            }

            helpMessage.AppendLine("Available Options");
            if(requiredOptions.Length > 0)
            {
                helpMessage.AppendLine("\tRequired");
                helpMessage.Append(requiredOptions);
            }

            if(optionalOptions.Length > 0)
            {
                helpMessage.AppendLine("\tOptional");
                helpMessage.Append(optionalOptions);
            }

            return helpMessage.ToString();
        }
    }
}
