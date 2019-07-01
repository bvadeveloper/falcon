using System.Collections.Generic;
using System.Threading.Tasks;
using Falcon.Profiles;
using Falcon.Services;
using Microsoft.AspNetCore.Mvc;

namespace Falcon.Api.Controllers
{
    [Route("api/scan")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IRequestProcessingService _processingService;

        public EmailController(IRequestProcessingService processingService)
        {
            _processingService = processingService;
        }

        // POST api/scan/emails
        [HttpPost("emails")]
        public async Task<string> ScanEmails([FromBody] List<string> emails)
        {
            return await _processingService.ScanEmailsAsync(emails).Extract();
        }
    }
}