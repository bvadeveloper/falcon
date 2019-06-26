using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Falcon.EasyNetQ.Module;
using Falcon.Logging.Scanner.Module;
using Falcon.Services;
using Microsoft.Extensions.Hosting;

namespace Falcon.Scanner
{
    static class Program
    {
        private static async Task Main() =>
            await new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule<ScannerLoggerModule>();
                    builder.RegisterModule<EasyNetQModule>();
                    builder.RegisterType<OrchestrationService>().As<IHostedService>();
                })
                .RunConsoleAsync();
    }
}