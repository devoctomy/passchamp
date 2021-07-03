using devoctomy.Passchamp.Windows.ViewModels;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace devoctomy.Passchamp.Windows.Services
{
    public class ViewModelLocator : IViewModelLocator
    {
        public MainViewModel MainViewModel
        {
            get
            {
                return Ioc.Default.GetRequiredService<MainViewModel>();
            }
        }

        public GraphTesterViewModel GraphTesterViewModel
        {
            get
            {
                return Ioc.Default.GetRequiredService<GraphTesterViewModel>();
            }
        }
    }
}
