using Falcon.Services.RequestProcessing;
using Microsoft.AspNetCore.Mvc;

namespace Falcon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GdprController : ControllerBase
    {
        private readonly IRequestProcessingService _processingService;

        public GdprController(IRequestProcessingService processingService)
        {
            _processingService = processingService;
        }
    }
}