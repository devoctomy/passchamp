using devoctomy.Passchamp.Windows.Model;
using devoctomy.Passchamp.Windows.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;

namespace devoctomy.Passchamp.Windows.ViewModels
{
    public class MainViewModel : ViewModelBase<MainModel>
    {
        public ICommand ShowGraphTester { get; }

        public MainViewModel(
            ILogger<MainViewModel> logger,
            MainModel model)
            : base(logger, model)
        {
            ShowGraphTester = new RelayCommand(DoShowGraphTester);
        }

        private void DoShowGraphTester()
        {
            Logger.LogInformation("Showing Graph Tester view...");
            var graphTester = new GraphTester();
            var result = graphTester.ShowDialog();
        }
    }
}
