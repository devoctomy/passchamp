using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace devoctomy.Passchamp.Windows.ViewModels
{
    public class ViewModelBase<T> : ObservableRecipient
    {
        protected System.Windows.Threading.Dispatcher Dispatcher { get; set; }

        public ILogger<ViewModelBase<T>> Logger { get; }
        public T Model { get; set; }

        public ViewModelBase(
            ILogger<ViewModelBase<T>> logger,
            T model)
        {
            Dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            Logger = logger;
            Model = model;
        }
    }
}
