using System.Threading.Tasks;

namespace Falcon.Hosts.Telegram
{
    internal static class Program
    {
        private static async Task Main() => await Host.InitBasic();
    }
}