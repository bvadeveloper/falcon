using System.Threading.Tasks;
using Falcon.Profiles;

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
}