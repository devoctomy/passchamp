using devoctomy.Passchamp.Core.Graph.Services;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace devoctomy.Passchamp.Windows.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        private readonly IGraphLoaderService _graphLoaderService;
        private string _testValue = "Bob";

        public string TestValue
        {
            get => _testValue;
            private set => SetProperty(ref _testValue, value);
        }

        public MainViewModel(IGraphLoaderService graphLoaderService)
        {
            _graphLoaderService = graphLoaderService;
        }
    }
}
