using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services;

public interface ICommandLineArgumentService
{
    string GetArguments(string fullCommandLine);
}
