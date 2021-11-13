using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services
{
    public interface IProgram
    {
        Task<int> Run();
    }
}
