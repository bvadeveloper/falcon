using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNetQ;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Profiles.Collect;
using Falcon.Profiles.Scan;

namespace Falcon.Services.RequestManagement
{
    public abstract class RequestManagement
    {
        private readonly IBus _bus;
        private readonly IJsonLogger _logger;
        private readonly SessionContext _context;

        protected RequestManagement(IBus bus, IJsonLogger<RequestManagement> logger, SessionContext context)
        {
            _bus = bus;
            _logger = logger;
            _context = context;
        }

        protected Task<Result<string>> Publish<TProfile>(List<string> targets,
            Func<string, SessionContext, TProfile> func)
            where TProfile : class
        {
            try
            {
                _logger.Trace("Processing", targets);

                targets.ForEach(async target => await _bus.PublishAsync(func(target, _context)));

                return Task.FromResult(new Result<string>()
                    .UseResult("requests in processing")
                    .Context(_context)
                    .Ok());
            }
            catch (Exception ex)
            {
                _logger.Error("Sending error", targets, ex);

                return Task.FromResult(new Result<string>()
                    .UseResult("request sending error")
                    .Context(_context)
                    .Fail());
            }
        }
    }

    public class RequestManagementService : RequestManagement, IRequestManagementService
    {
        public RequestManagementService(IBus bus, IJsonLogger<RequestManagementService> logger)
            : base(bus, logger, new SessionContext { ClientName = "client", SessionId = Guid.NewGuid() }) { }

        public Task<Result<string>> ScanIpAsync(List<string> targets, List<string> tools)
        {
            return Publish(targets, (target, context) =>
                new IpScanProfile
                {
                    Target = target, Tools = tools, Context = context
                });
        }

        public Task<Result<string>> ScanDomainsAsync(List<string> targets, List<string> tools)
        {
            return Publish(targets, (target, context) =>
                new DomainCollectProfile
                {
                    Target = target, Tools = tools, Context = context
                });
        }

        public Task<Result<string>> ScanEmailsAsync(List<string> targets)
        {
            return Publish(targets, (target, context) =>
                new EmailScanProfile { Target = target, Context = context });
        }

        public Task<Result<string>> ScanGdprInfoAsync(List<string> targets)
        {
            return Publish(targets, (target, context) =>
                new GdprScanProfile { Target = target, Context = context });
        }
    }
}