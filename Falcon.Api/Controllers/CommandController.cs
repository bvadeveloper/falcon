using System.Net;
using System.Threading.Tasks;
using Falcon.Profiles;
using Falcon.Services.RequestManagement;
using Microsoft.AspNetCore.Mvc;

namespace Falcon.Api.Controllers
{
    [Route("api/scan")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly IRequestManagementService _processingService;

        public CommandController(IRequestManagementService processingService)
        {
            _processingService = processingService;
        }

        // POST api/scan/ip
        [HttpPost("ip")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<Result<string>> ScanIp([FromBody] RequestModel model) =>
            await _processingService.IpScanAsync(model);

        // POST api/scan/domains
        [HttpPost("domains")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<Result<string>> ScanDomains([FromBody] RequestModel model) =>
            await _processingService.DomainsVulnerabilityScanAsync(model);

        // POST api/scan/emails
        [HttpPost("emails")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<Result<string>> ScanEmails([FromBody] RequestModel model) =>
            await _processingService.MailboxLeakCheckAsync(model);

        
        
    }
}