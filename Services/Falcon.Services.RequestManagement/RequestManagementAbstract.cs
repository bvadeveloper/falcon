using System;
using System.Threading.Tasks;
using EasyNetQ;
using Falcon.Logging;
using Falcon.Profiles;

namespace Falcon.Services.RequestManagement {
    public abstract class RequestManagementAbstract
    {
        private readonly IBus _bus;
        private readonly IJsonLogger _logger;
        private readonly SessionContext _context;

        protected RequestManagementAbstract(IBus bus, IJsonLogger<RequestManagementAbstract> logger, SessionContext context)
        {
            _bus = bus;
            _logger = logger;
            _context = context;
        }

        protected async Task<Result<string>> Publish<TProfile>(TargetModel targetModel)
            where TProfile : class, IProfile, new()
        {
            try
            {
                _logger.Trace("Processing", targetModel);

                foreach (var target in targetModel.Targets)
                {
                    var profile = new TProfile { Context = _context, Target = target, Tools = targetModel.Tools };
                    await _bus.PublishAsync(profile);
                }

                return new Result<string>()
                    .UseResult("requests in processing")
                    .Context(_context)
                    .Ok();
            }
            catch (Exception ex)
            {
                _logger.Error("Sending error", targetModel, ex);

                return new Result<string>()
                    .UseResult("request sending error")
                    .Context(_context)
                    .Fail();
            }
        }
    }
}