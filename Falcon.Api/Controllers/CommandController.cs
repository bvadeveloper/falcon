using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Falcon.Api.Models;
using Falcon.Services;
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
        public async Task<Result<string>> ScanIp([FromBody] TargetModel model)
        {
            return await _processingService.ScanIpAsync(model.Targets, model.Tools);
        }

        // POST api/scan/domains
        [HttpPost("domains")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<Result<string>> ScanDomains([FromBody] TargetModel model)
        {
            return await _processingService.ScanDomainsAsync(model.Targets, model.Tools);
        }

        // POST api/scan/emails
        [HttpPost("emails")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<Result<string>> ScanEmails([FromBody] List<string> emails)
        {
            return await _processingService.ScanEmailsAsync(emails);
        }

        // POST api/scan/gdpr
        [HttpPost("gdpr")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<Result<string>> ScanGdpr([FromBody] List<string> domains)
        {
            return await _processingService.ScanGdprInfoAsync(domains);
        }
    }
}