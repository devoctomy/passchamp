using System;
using System.Windows.Forms;

namespace devoctomy.Passchamp.Views
{
    public class ViewLocator : IViewLocator
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewLocator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T CreateInstance<T>() where T : Form
        {
            return (T)_serviceProvider.GetService(typeof(T));
        }
    }
}
