using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EasyNetQ.AutoSubscribe;
using Falcon.Bus.EasyNetQ;
using Falcon.Bus.EasyNetQ.Module;
using Falcon.Logging.Scan.Module;
using Falcon.Services.Tool;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Falcon.Hosts
{
    public static class Host
    {
        public static Task Init()
        {
            return new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.AddEnvironmentVariables();
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule<ScanLoggerModule>();
                    builder.RegisterModule<BusModule>();
                    builder.RegisterType<HostedService>().As<IHostedService>();
                    builder.RegisterType<ToolService>().As<IToolService>();
                    builder.RegisterType<MessageDispatcher>().As<IAutoSubscriberMessageDispatcher>();
                    builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                        .Where(t => t.Name.EndsWith("Consumer")).AsSelf();
                })
                .RunConsoleAsync();
        }
    }
}