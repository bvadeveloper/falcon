using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Falcon.Bus.EasyNetQ;
using Falcon.Logging;
using Microsoft.Extensions.Hosting;

namespace Falcon.Hosts
{
    public class HostedService : IHostedService
    {
        private readonly IJsonLogger _logger;
        private readonly IBusSubscriber _busSubscriber;

        public HostedService(
            IJsonLogger<HostedService> logger,
            IBusSubscriber busSubscriber)
        {
            _logger = logger;
            _busSubscriber = busSubscriber;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Information($"Host start '{Assembly.GetEntryAssembly()?.GetName().Name}'");

            _busSubscriber.Subscribe();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Information($"Host stop '{Assembly.GetEntryAssembly()?.GetName().Name}'");

            return Task.CompletedTask;
        }
    }
}