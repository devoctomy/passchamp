using System;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services;

public interface IProgram
{
    Func<string> GetCommandLine { get; set; }
    Task<int> Run();
}
