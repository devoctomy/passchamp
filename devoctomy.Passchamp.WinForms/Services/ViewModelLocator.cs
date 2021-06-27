using devoctomy.Passchamp.Models;
using System;

namespace devoctomy.Passchamp.Services
{
    public class ViewModelLocator : IViewModelLocator
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelLocator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T CreateInstance<T>() where T : ViewModelBase
        {
            return (T)_serviceProvider.GetService(typeof(T));
        }
    }
}
