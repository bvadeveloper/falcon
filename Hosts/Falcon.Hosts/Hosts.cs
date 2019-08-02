using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Falcon.Bus.EasyNetQ.Module;
using Falcon.Data.Redis.Module;
using Falcon.Logging.Report.Module;
using Falcon.Logging.Scan.Module;
using Falcon.Tools.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Falcon.Hosts
{
    public static class Host
    {
        /// <summary>
        /// Init scanning hosts
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Task Init(Action<ContainerBuilder> action = default)
        {
            return new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder
                        .SetBasePath(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName)
                        .AddJsonFile("tools.json", optional: false, reloadOnChange: true);

                    configurationBuilder.AddEnvironmentVariables();
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule<ScanLoggerModule>();
                    builder.RegisterModule<BusSubscriberModule>();
                    builder.RegisterModule<RedisModule>();
                    builder.RegisterType<BusHostedService>().As<IHostedService>();

                    // register all stuff for tools
                    builder.RegisterModule<ToolModule>();

                    action?.Invoke(builder);
                })
                .RunConsoleAsync();
        }

        /// <summary>
        /// Init basic hosts
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Task InitBasic(Action<ContainerBuilder> action = default)
        {
            return new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.AddEnvironmentVariables();
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule<ReportLoggerModule>();
                    builder.RegisterModule<BusSubscriberModule>();
                    builder.RegisterType<BusHostedService>().As<IHostedService>();

                    action?.Invoke(builder);
                })
                .RunConsoleAsync();
        }
    }
}