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
    public static class Program
    {
        static async Task<int> Main()
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
                    .AddSingleton<IGenerateService, GenerateService>()
                    .AddSingleton<IRsaJsonSignerService, RsaJsonSignerService>()
                    .AddSingleton<IRsaJsonVerifierService, RsaJsonVerifierService>()
                    .AddSingleton<IHelpMessageFormatter, HelpMessageFormatter>()
                    .AddSingleton<IProgram, SignToolProgram>());
    }
}
