using devoctomy.Passchamp.Windows.ViewModels;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace devoctomy.Passchamp.Windows.Services
{
    public class ViewModelLocator : IViewModelLocator
    {
        public MainViewModel MainViewModel => Ioc.Default.GetRequiredService<MainViewModel>();

        public GraphTesterViewModel GraphTesterViewModel => Ioc.Default.GetRequiredService<GraphTesterViewModel>();
    }
}
