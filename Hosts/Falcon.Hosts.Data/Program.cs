using System.Threading.Tasks;

namespace Falcon.Hosts.Data
{
    internal static class Program
    {
        private static async Task Main() => await Host.InitBasic();
    }
}