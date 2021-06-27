using devoctomy.Passchamp.Models;

namespace devoctomy.Passchamp.Services
{
    public interface IViewModelLocator
    {
        T CreateInstance<T>() where T : ViewModelBase;
    }
}
