using System.Threading.Tasks;
using Autofac;
using Falcon.Messengers.Telegram.Module;

namespace Falcon.Hosts.Telegram
{
    internal static class Program
    {
        private static Task Main() => Host.InitBasic(builder => builder.RegisterModule<TelegramModule>());
    }
}