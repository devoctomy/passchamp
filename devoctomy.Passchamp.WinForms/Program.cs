using devoctomy.Passchamp.Core.Extensions;
using devoctomy.Passchamp.Dialogs;
using devoctomy.Passchamp.Models;
using devoctomy.Passchamp.Services;
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
            ConfigureServices(services);
            services.AddPasschampCoreServices();
            using var serviceProvider = services.BuildServiceProvider();
            var mainForm = serviceProvider.GetRequiredService<Main>();
            Application.Run(mainForm);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole());
            services.AddScoped<MainViewModel>();
            services.AddSingleton<IViewModelLocator, ViewModelLocator>();
            services.AddSingleton<IViewLocator, ViewLocator>();
            services.AddScoped<Main>();
            services.AddScoped<GraphTesterDialog>();
        }
    }
}
