using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Falcon.Bus.EasyNetQ.Module;
using Falcon.Logging.Sorter.Module;
using Falcon.Services.Scanning;
using Microsoft.Extensions.Hosting;

namespace Falcon.Sorter
{
    static class Program
    {
        private static async Task Main() =>
            await new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule<SorterLoggerModule>();
                    builder.RegisterModule<EasyNetQModule>();
                    builder.RegisterType<HostedService>().As<IHostedService>();
                })
                .RunConsoleAsync();
    }
}