using Falcon.Services.RequestProcessing;
using Microsoft.AspNetCore.Mvc;

namespace Falcon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScanController : ControllerBase
    {
        private readonly IRequestProcessingService _processingService;

        public ScanController(IRequestProcessingService processingService)
        {
            _processingService = processingService;
        }
    }
}