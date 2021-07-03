using devoctomy.Passchamp.Windows.Model;
using Microsoft.Extensions.Logging;

namespace devoctomy.Passchamp.Windows.ViewModels
{
    public class GraphTesterViewModel : ViewModelBase<GraphTesterModel>
    {
        public GraphTesterViewModel(
            ILogger<GraphTesterViewModel> logger,
            GraphTesterModel model)
            : base(logger, model)
        {
        }
    }
}
