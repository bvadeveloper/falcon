using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Falcon.Bus.EasyNetQ.Module;
using Falcon.Logging.Scan.Module;
using Microsoft.Extensions.Hosting;

namespace Falcon.Hosts.Scanner
{
    internal static class Program
    {
        private static async Task Main() => await Host.Init();
    }
}