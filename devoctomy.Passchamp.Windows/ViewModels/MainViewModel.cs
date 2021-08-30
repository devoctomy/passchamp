using devoctomy.Passchamp.Windows.Model;
using devoctomy.Passchamp.Windows.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;

namespace devoctomy.Passchamp.Windows.ViewModels
{
    public class MainViewModel : ViewModelBase<MainModel>
    {
        private readonly IViewLocator _viewLocator;

        public ICommand ShowGraphTester { get; }

        public MainViewModel(
            ILogger<MainViewModel> logger,
            MainModel model,
            IViewLocator viewLocator)
            : base(logger, model)
        {
            ShowGraphTester = new RelayCommand(DoShowGraphTester);
            _viewLocator = viewLocator;
        }

        private void DoShowGraphTester()
        {
            Logger.LogInformation("Showing Graph Tester view...");
            var graphTester = _viewLocator.GraphTester;
            var result = graphTester.ShowDialog();
        }
    }
}
