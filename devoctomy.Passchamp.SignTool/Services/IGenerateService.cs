using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool.Services
{
    public interface IGenerateService
    {
        Task<int> Generate(GenerateOptions options);
    }
}
