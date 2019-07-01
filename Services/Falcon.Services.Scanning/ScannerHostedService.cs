using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Falcon.Logging;
using Falcon.Contracts;
using Microsoft.Extensions.Hosting;

namespace Falcon.Services.Scanning
{
    public class HostedService : IHostedService
    {
        private readonly IJsonLogger _logger;
        private readonly IBus _bus;
        private readonly IRequestHandler _requestHandler;

        public HostedService(
            IBus bus,
            IRequestHandler requestHandler,
            IJsonLogger<HostedService> logger)
        {
            _logger = logger;
            _bus = bus;
            _requestHandler = requestHandler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            _logger.Information("Start");

            _bus.SubscribeAsync<TargetProfile>(string.Empty, // todo: set id
                async script => await _requestHandler.ProcessAsync(script));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            _logger.Information("Stop");

            _bus.Dispose();
        }
    }
}