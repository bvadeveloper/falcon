using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Falcon.Bus.EasyNetQ.Module;
using Falcon.Logging.Сollector.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Falcon.Hosts.Сollector
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
                    builder.RegisterModule<СollectorLoggerModule>();
                    builder.RegisterModule<EasyNetQModule>();
                    builder.RegisterType<HostedService>().As<IHostedService>();
                })
                .RunConsoleAsync();
    }
}