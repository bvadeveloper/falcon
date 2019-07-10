using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Falcon.Bus.EasyNetQ;
using Falcon.Logging;
using Microsoft.Extensions.Hosting;

namespace Falcon.Hosts
{
    public class BusHostedService : IHostedService
    {
        private readonly IJsonLogger _logger;
        private readonly IBusSubscriber _busSubscriber;

        public BusHostedService(
            IJsonLogger<BusHostedService> logger,
            IBusSubscriber busSubscriber)
        {
            _logger = logger;
            _busSubscriber = busSubscriber;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _busSubscriber.Subscribe();
            _logger.Information($"Host started '{Assembly.GetEntryAssembly()?.GetName().Name}'");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _busSubscriber.Unsubscribe();
            _logger.Information($"Host stopped '{Assembly.GetEntryAssembly()?.GetName().Name}'");

            return Task.CompletedTask;
        }
    }
}