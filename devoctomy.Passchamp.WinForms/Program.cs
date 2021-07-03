using devoctomy.Passchamp.Core.Extensions;
using devoctomy.Passchamp.Dialogs;
using devoctomy.Passchamp.Models;
using devoctomy.Passchamp.Services;
using devoctomy.Passchamp.ViewModels;
using devoctomy.Passchamp.Views;
using devoctomy.Passchamp.WinForms.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Forms;

namespace devoctomy.Passchamp.WinForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureDependencyInjection(services);
            using var serviceProvider = services.BuildServiceProvider();
            var mainForm = serviceProvider.GetRequiredService<Main>();
            Application.Run(mainForm);
        }

        private static void ConfigureDependencyInjection(IServiceCollection services)
        {
            ConfigureLogging(services);
            ConfigureModels(services);
            ConfigureViewModels(services);
            ConfigureViews(services);
            ConfigureServices(services);
        }

        private static void ConfigureLogging(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole());
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

        private static void ConfigureViews(IServiceCollection services)
        {
            services.AddScoped<Main>();
            services.AddScoped<GraphTesterDialog>();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IViewLocator, ViewLocator>();
            services.AddPasschampCoreServices();
        }
    }
}
