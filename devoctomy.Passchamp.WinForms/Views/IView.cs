using Microsoft.Extensions.Logging;

namespace devoctomy.Passchamp.Views
{
    public interface IView<ViewType, ViewModelType>
    {
        ILogger<ViewType> Logger { get; }
        ViewModelType ViewModel { get; }
    }
}
