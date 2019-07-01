using System.Threading.Tasks;
using Falcon.Contracts;
using Falcon.Services.RequestProcessing;
using Microsoft.AspNetCore.Mvc;

namespace Falcon.Api.Controllers
{
    [Route("api/scan")]
    [ApiController]
    public class GdprController : ControllerBase
    {
        private readonly IRequestProcessingService _processingService;

        public GdprController(IRequestProcessingService processingService)
        {
            _processingService = processingService;
        }

        // POST api/scan/gdpr
        [HttpPost("gdpr")]
        public async Task<string> ScanGdpr([FromBody] string domain)
        {
            return await _processingService.ScanGdprInfoAsync(domain).Extract();
        }
    }
}