using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Falcon.Bus.EasyNetQ;
using Falcon.Logging;
using Microsoft.Extensions.Hosting;

namespace Falcon.Hosts
{
    public class HostedService : IHostedService
    {
        private readonly IJsonLogger _logger;
        private readonly IBus _bus;
        private readonly IAutoSubscriberMessageDispatcher _messageDispatcher;

        public HostedService(
            IBus bus,
            IJsonLogger<HostedService> logger,
            IAutoSubscriberMessageDispatcher messageDispatcher)
        {
            _logger = logger;
            _messageDispatcher = messageDispatcher;
            _bus = bus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Information($"Host start '{Assembly.GetEntryAssembly()?.GetName().Name}'");

            var subscriber = new AutoSubscriber(_bus, "_")
            {
                ConfigureSubscriptionConfiguration = s => s.WithAutoDelete(),
                GenerateSubscriptionId = info => "_",
                AutoSubscriberMessageDispatcher = _messageDispatcher
            };

            subscriber.SubscribeAsync(Assembly.GetEntryAssembly());

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