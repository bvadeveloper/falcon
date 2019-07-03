using System;
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

        public Task<Result<string>> ScanIpAsync(List<string> targets, List<string> tools)
        {
            targets.Publish(_bus, t => new IpScanProfile
            {
                Target = t,
                Tools = tools
            });

            return Task.FromResult(new Result<string>().SetResult("request in processing").Ok());
        }


        public Task<Result<string>> ScanDomainsAsync(List<string> targets, List<string> tools)
        {
            targets.Publish(_bus, t => new DomainCollectProfile
            {
                Target = t,
                Tools = tools
            });

            return Task.FromResult(new Result<string>().SetResult("request in processing").Ok());
        }

        public Task<Result<string>> ScanEmailsAsync(List<string> targets)
        {
            targets.Publish(_bus, t => new EmailScanProfile
            {
                Target = t,
            });

            return Task.FromResult(new Result<string>().SetResult("request in processing").Ok());
        }

        public Task<Result<string>> ScanGdprInfoAsync(List<string> targets)
        {
            targets.Publish(_bus, t => new GdprScanProfile
            {
                Target = t,
            });

            return Task.FromResult(new Result<string>().SetResult("request in processing").Ok());
        }
    }

    internal static class PublishExtensions
    {
        internal static void Publish<TProfile>(this List<string> targets, IBus bus, Func<string, TProfile> func)
            where TProfile : class => targets.ForEach(async t => await bus.PublishAsync(func(t)));
    }
}