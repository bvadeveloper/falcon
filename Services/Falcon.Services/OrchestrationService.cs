using System.Threading;
using System.Threading.Tasks;
using Falcon.Logging.Scanner;
using Microsoft.Extensions.Hosting;

namespace Falcon.Services
{
    public class OrchestrationService : IHostedService
    {
        private readonly IScannerLogger _scannerLogger;

        public OrchestrationService(IScannerLogger<OrchestrationService> scannerLogger)
        {
            _scannerLogger = scannerLogger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            _scannerLogger.Information("Start");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            _scannerLogger.Information("Stop");
        }
    }
}