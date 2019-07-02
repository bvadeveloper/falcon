using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNetQ;
using Falcon.Logging;
using Falcon.Profiles.Collect;
using Falcon.Profiles.Scan;

namespace Falcon.Services.RequestProcessing
{
    public class RequestProcessingService : IRequestProcessingService
    {
        private readonly IJsonLogger _logger;
        private readonly IBus _bus;

        public RequestProcessingService(IJsonLogger<RequestProcessingService> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task<Result<string>> ScanIpAsync(List<string> targets, List<string> tools)
        {
            await _bus.PublishAsync(new ScanIpProfile
            {
                Targets = targets,
                Tools = tools
            });

            return new Result<string>().SetResult("request in processing");
        }

        public async Task<Result<string>> ScanDomainsAsync(List<string> domains, List<string> tools)
        {
            await _bus.PublishAsync(new CollectDomainProfile
            {
                Targets = domains,
                Tools = tools
            });

            return new Result<string>().SetResult("request in processing").Ok();
        }

        public async Task<Result<string>> ScanEmailsAsync(List<string> emails)
        {
            await _bus.PublishAsync(new ScanEmailProfile
            {
                Targets = emails
            });

            return new Result<string>().SetResult("request in processing").Ok();
        }

        public async Task<Result<string>> ScanGdprInfoAsync(List<string> domains)
        {
            await _bus.PublishAsync(new ScanGdprProfile
            {
                Targets = domains
            });

            return new Result<string>().SetResult("request in processing").Ok();
        }
    }
}