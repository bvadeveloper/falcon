using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Falcon.Logging;
using Microsoft.Extensions.Hosting;

namespace Falcon.Hosts
{
    public class HostedService : IHostedService
    {
        private readonly IJsonLogger _logger;
        private readonly IBus _bus;

        public HostedService(IBus bus, IJsonLogger<HostedService> logger)
        {
            _logger = logger;
            _bus = bus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Information($"Host start '{Assembly.GetEntryAssembly()?.GetName().Name}'");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Information($"Host stop '{Assembly.GetEntryAssembly()?.GetName().Name}'");
            _bus.Dispose();

            return Task.CompletedTask;
        }
    }
}