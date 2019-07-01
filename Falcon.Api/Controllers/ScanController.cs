using System.Collections.Generic;
using System.Threading.Tasks;
using Falcon.Profiles;
using Falcon.Services;
using Microsoft.AspNetCore.Mvc;

namespace Falcon.Api.Controllers
{
    [Route("api/scan")]
    [ApiController]
    public class ScanController : ControllerBase
    {
        private readonly IRequestProcessingService _processingService;

        public ScanController(IRequestProcessingService processingService)
        {
            _processingService = processingService;
        }

        // POST api/scan/ip
        [HttpPost("ip")]
        public async Task<Result> ScanIp([FromBody] string ip)
        {
            return await _processingService.ScanIpAsync(ip);
        }

        // POST api/scan/domains
        [HttpPost("domains")]
        public async Task<Result> ScanDomains([FromBody] List<string> domains, List<string> tools)
        {
            return await _processingService.ScanDomainsAsync(domains, tools);
        }
    }
}