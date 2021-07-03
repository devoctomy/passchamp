using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace devoctomy.Passchamp.Windows.ViewModels
{
    public class ViewModelBase<T> : ObservableRecipient
    {
        public ILogger<ViewModelBase<T>> Logger { get; }
        public T Model { get; }

        public ViewModelBase(
            ILogger<ViewModelBase<T>> logger,
            T model)
        {
            Logger = logger;
            Model = model;
        }
    }
}
