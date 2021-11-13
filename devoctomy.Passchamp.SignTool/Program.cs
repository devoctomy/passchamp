using devoctomy.Passchamp.SignTool.Services;
using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.SignTool
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        static async Task<int> Main(string[] args)
        {
            using IHost host = CreateHostBuilder(null).Build();

            var program = host.Services.GetService<IProgram>(); ;
            return await program.Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
                services
                    .AddSingleton<ICommandLineArgumentService, CommandLineArgumentsService>()
                    .AddSingleton<ICommandLineParserService, CommandLineParserService>((IServiceProvider _) => { return CommandLineParserService.CreateDefaultInstance(); })
                    .AddSingleton<IProgram, SignToolProgram>());
    }
}
