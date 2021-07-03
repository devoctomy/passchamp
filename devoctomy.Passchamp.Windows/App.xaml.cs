using devoctomy.Passchamp.Core.Extensions;
using devoctomy.Passchamp.Windows.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
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
            services.AddPasschampCoreServices();
            services.AddScoped<MainViewModel>();
            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }
    }
}
