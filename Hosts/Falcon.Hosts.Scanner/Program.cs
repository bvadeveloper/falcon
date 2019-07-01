using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Falcon.Bus.EasyNetQ.Module;
using Falcon.Logging.Scanner.Module;
using Falcon.Services.Scanning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Falcon.Hosts.Scanner
{
    static class Program
    {
        private static async Task Main() =>
            await new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.AddEnvironmentVariables();
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule<ScannerLoggerModule>();
                    builder.RegisterModule<EasyNetQModule>();
                    builder.RegisterType<HostedService>().As<IHostedService>();
                })
                .RunConsoleAsync();
    }
}