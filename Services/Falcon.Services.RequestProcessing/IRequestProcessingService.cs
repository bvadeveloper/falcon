using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcon.Services.RequestProcessing
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
}