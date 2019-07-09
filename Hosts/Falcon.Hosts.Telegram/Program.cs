using System.Threading.Tasks;
using Autofac;
using Falcon.Services.Telegram;
using Microsoft.Extensions.Hosting;

namespace Falcon.Hosts.Telegram
{
    internal static class Program
    {
        private static async Task Main() => await Host.InitBasic(builder =>
        {
            builder.RegisterType<TelegramResponseHostedService>().As<IHostedService>();
        });
    }
}