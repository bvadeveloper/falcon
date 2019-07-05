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
        public async Task<Result<string>> ScanIp([FromBody] TargetModel model) =>
            await _processingService.ScanIpAsync(model);

        // POST api/scan/domains
        [HttpPost("domains")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<Result<string>> ScanDomains([FromBody] TargetModel model) =>
            await _processingService.ScanDomainsAsync(model);

        // POST api/scan/emails
        [HttpPost("emails")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<Result<string>> ScanEmails([FromBody] TargetModel model) =>
            await _processingService.ScanEmailsAsync(model);

        // POST api/scan/gdpr
        [HttpPost("gdpr")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<Result<string>> ScanGdpr([FromBody] TargetModel model) =>
            await _processingService.ScanGdprInfoAsync(model);
    }
}