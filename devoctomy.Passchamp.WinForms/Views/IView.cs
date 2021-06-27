using Microsoft.Extensions.Logging;

namespace devoctomy.Passchamp.Views
{
    public interface IView<ViewType, ModelType>
    {
        ILogger<ViewType> Logger { get; }
        ModelType Model { get; }
    }
}
