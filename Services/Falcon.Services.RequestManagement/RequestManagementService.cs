using System.Threading.Tasks;
using EasyNetQ;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Profiles.Collect;
using Falcon.Profiles.Scan;

namespace Falcon.Services.RequestManagement
{
    public interface IRequestManagementService
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
        Task<Result<string>> IpScanAsync(RequestModel model);

        Task<Result<string>> DomainsVulnerabilityScanAsync(RequestModel model);

        Task<Result<string>> MailboxLeakCheckAsync(RequestModel model);
    }

    public class RequestManagementService : RequestManagementAbstract, IRequestManagementService
    {
        public RequestManagementService(
            IBus bus,
            IJsonLogger<RequestManagementService> logger,
            IContext sessionContext)
            : base(bus, logger, sessionContext) { }

        public Task<Result<string>> IpScanAsync(RequestModel model) =>
            Publish<IpScanProfile>(model);

        public Task<Result<string>> DomainsVulnerabilityScanAsync(RequestModel model) =>
            Publish<DomainCollectProfile>(model);

        public Task<Result<string>> MailboxLeakCheckAsync(RequestModel model) =>
            Publish<EmailScanProfile>(model);
    }
}