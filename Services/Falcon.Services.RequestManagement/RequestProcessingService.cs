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
    public interface IRequestProcessingService
    {
        // todo: only for ports
        // 21 File Transfer(FTP)
        // 22 Secure Shell(SSH)
        // 23 Telnet
        // 25 Mail(SMTP)
        // 80 Web(HTTP)
        // 110 Mail(POP3)
        // 143 Mail(IMAP)
        // 443 SSL/TLS(HTTPS)
        // 445 Microsoft(SMB)
        // 3389 Remote(RDP)
        Task<Result<string>> ScanIpAsync(List<string> targets, List<string> tools);

        Task<Result<string>> ScanDomainsAsync(List<string> targets, List<string> tools);

        Task<Result<string>> ScanEmailsAsync(List<string> emails);

        Task<Result<string>> ScanGdprInfoAsync(List<string> domains);
    }

    public class RequestProcessingService : IRequestProcessingService
    {
        private readonly IJsonLogger _logger;
        private readonly IBus _bus;

        public RequestProcessingService(IBus bus, IJsonLogger<RequestProcessingService> logger)
        {
            _logger = logger;
            _bus = bus;
        }

        public Task<Result<string>> ScanIpAsync(List<string> targets, List<string> tools) =>
            Publish(targets, (target, context) =>
                new IpScanProfile
                {
                    Target = target, Tools = tools, Context = context
                });

        public Task<Result<string>> ScanDomainsAsync(List<string> targets, List<string> tools) =>
            Publish(targets, (target, context) =>
                new DomainCollectProfile
                {
                    Target = target, Tools = tools, Context = context
                });

        public Task<Result<string>> ScanEmailsAsync(List<string> targets) =>
            Publish(targets, (target, context) =>
                new EmailScanProfile { Target = target, Context = context });

        public Task<Result<string>> ScanGdprInfoAsync(List<string> targets) =>
            Publish(targets, (target, context) =>
                new GdprScanProfile { Target = target, Context = context });

        private Task<Result<string>> Publish<TProfile>(List<string> targets,
            Func<string, SessionContext, TProfile> func)
            where TProfile : class
        {
            var context = new SessionContext { ClientName = "client", SessionId = Guid.NewGuid() };

            try
            {
                _logger.Trace("Requests in processing", targets);
                targets.ForEach(async target => await _bus.PublishAsync(func(target, context)));

                return Task.FromResult(new Result<string>()
                    .SetResult("requests in processing")
                    .Context(context)
                    .Ok());
            }
            catch (Exception ex)
            {
                _logger.Error("Request sending error", targets, ex);

                return Task.FromResult(new Result<string>()
                    .SetResult("request sending error")
                    .Context(context)
                    .Fail());
            }
        }
    }
}