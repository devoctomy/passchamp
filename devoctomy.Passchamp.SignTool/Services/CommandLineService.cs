using System;
using System.Reflection;

namespace devoctomy.Passchamp.SignTool.Services;

public class CommandLineArgumentsService : ICommandLineArgumentService
{
    public string GetArguments(string fullCommandLine)
    {
        var curExePath = Assembly.GetEntryAssembly().Location;
        var arguments = fullCommandLine.Replace(curExePath, string.Empty).Trim();
        return arguments;
    }
}
