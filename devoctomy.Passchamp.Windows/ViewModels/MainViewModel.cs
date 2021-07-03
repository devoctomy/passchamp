using devoctomy.Passchamp.Core.Graph.Services;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Windows.Input;

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

        public ICommand Test { get; }

        public MainViewModel(IGraphLoaderService graphLoaderService)
        {
            _graphLoaderService = graphLoaderService;
            Test = new RelayCommand(DoStuff, DoStuffCanExecute);
        }

        private bool DoStuffCanExecute()
        {
            return true;
        }

        private void DoStuff()
        {
            TestValue = Guid.NewGuid().ToString();
        }
    }
}
