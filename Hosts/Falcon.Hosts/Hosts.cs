using System.IO;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Falcon.Bus.EasyNetQ.Module;
using Falcon.Data.Redis;
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
        /// <returns></returns>
        public static Task Init()
        {
            return new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("tools.json", optional: false, reloadOnChange: true);

                    configurationBuilder.AddEnvironmentVariables();
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule<ScanLoggerModule>();
                    builder.RegisterModule<BusSubscriberModule>();
                    builder.RegisterModule<RedisModule>();
                    builder.RegisterType<HostedService>().As<IHostedService>();

                    // register all stuff for tools
                    builder.RegisterModule<ToolModule>();
                })
                .RunConsoleAsync();
        }

        /// <summary>
        /// Init basic hosts
        /// </summary>
        /// <returns></returns>
        public static Task InitBasic()
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
                    builder.RegisterType<HostedService>().As<IHostedService>();
                })
                .RunConsoleAsync();
        }
    }
}