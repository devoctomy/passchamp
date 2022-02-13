using devoctomy.Passchamp.Core.Extensions;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Windows.Model;
using devoctomy.Passchamp.Windows.Services;
using devoctomy.Passchamp.Windows.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Linq;
using System.Windows;

namespace devoctomy.Passchamp.Windows
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;

        public App()
        {
            ConfigureServices();
            InitializeComponent();
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddLogging(configure => configure.AddConsole());
            services.AddPasschampCoreServices();
            ConfigureModels(services);
            ConfigureViewModels(services);
            ConfigureServices(services);
            AddNodes(services);
            var serviceProvider = services.BuildServiceProvider();
            Ioc.Default.ConfigureServices(serviceProvider);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IFileDialogService, FileDialogService>();
            services.AddScoped<IViewLocator, ViewLocator>();
        }

        private static void ConfigureModels(IServiceCollection services)
        {
            services.AddScoped<MainModel>();
            services.AddScoped<GraphTesterModel>();
        }

        private static void ConfigureViewModels(IServiceCollection services)
        {
            services.AddScoped<MainViewModel>();
            services.AddScoped<GraphTesterViewModel>();
        }

        private static void AddNodes(IServiceCollection services)
        {
            var nodeAssembly = typeof(App).Assembly;
            var allNodes = nodeAssembly.GetTypes().Where(x => typeof(INode).IsAssignableFrom(x) && !x.IsInterface).ToList();
            foreach (var node in allNodes)
            {
                services.AddScoped(typeof(INode), node);
                services.AddScoped(node);
            }
        }
    }
}
