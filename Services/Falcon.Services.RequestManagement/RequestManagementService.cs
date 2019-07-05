using System;
using System.Threading.Tasks;
using EasyNetQ;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Profiles.Collect;
using Falcon.Profiles.Scan;

namespace Falcon.Services.RequestManagement
{
    public class RequestManagementService : RequestManagementAbstract, IRequestManagementService
    {
        public RequestManagementService(IBus bus, IJsonLogger<RequestManagementService> logger)
            : base(bus, logger, new SessionContext { ClientName = "client", SessionId = Guid.NewGuid() }) { }

        public Task<Result<string>> ScanIpAsync(TargetModel model) =>
            Publish<IpScanProfile>(model);

        public Task<Result<string>> ScanDomainsAsync(TargetModel model) =>
            Publish<DomainCollectProfile>(model);

        public Task<Result<string>> ScanEmailsAsync(TargetModel model) =>
            Publish<EmailScanProfile>(model);

        public Task<Result<string>> ScanGdprInfoAsync(TargetModel model) =>
            Publish<GdprScanProfile>(model);
    }
}