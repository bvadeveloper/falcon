using System.Threading;
using System.Threading.Tasks;
using Falcon.Logging;
using Microsoft.Extensions.Hosting;

namespace Falcon.Services
{
    public class OrchestrationService : IHostedService
    {
        private readonly IJsonLogger _logger;

        public OrchestrationService(IJsonLogger<OrchestrationService> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            _logger.Information("Start");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            _logger.Information("Stop");
        }
    }
}