using System.Collections.Generic;
using System.Threading.Tasks;
using Falcon.Contracts;
using Falcon.Services.RequestProcessing;
using Microsoft.AspNetCore.Mvc;

namespace Falcon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IRequestProcessingService _processingService;

        public EmailsController(IRequestProcessingService processingService)
        {
            _processingService = processingService;
        }

        // POST api/scanEmails
        [HttpPost]
        public async Task<string> ScanEmails([FromBody] List<string> emails)
        {
            return await _processingService.ScanEmailsAsync(emails).ExtractValue();
        }
    }
}