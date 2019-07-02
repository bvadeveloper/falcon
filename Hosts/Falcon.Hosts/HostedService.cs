using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
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
            _logger.Information("Start");

            var subscriber = new AutoSubscriber(_bus, "host")
            {
                ConfigureSubscriptionConfiguration = c => c.WithAutoDelete(),
                GenerateSubscriptionId = info => "host"
            };

            subscriber.SubscribeAsync(Assembly.GetEntryAssembly());

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Stop");
            _bus.Dispose();

            return Task.CompletedTask;
        }
    }
}