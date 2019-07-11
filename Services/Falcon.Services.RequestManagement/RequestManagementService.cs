﻿using System.Threading.Tasks;
using EasyNetQ;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Profiles.Collect;
using Falcon.Profiles.Scan;

namespace Falcon.Services.RequestManagement
{
    public class RequestManagementService : RequestManagementAbstract, IRequestManagementService
    {
        public RequestManagementService(
            IBus bus,
            IJsonLogger<RequestManagementService> logger,
            IContext sessionContext)
            : base(bus, logger, sessionContext) { }

        public Task<Result<string>> IpScanAsync(RequestModel model) =>
            Publish<IpScanProfile>(model);

        public Task<Result<string>> DomainsVulnerabilityScanAsync(RequestModel model) =>
            Publish<DomainCollectProfile>(model);

        public Task<Result<string>> MailboxLeakCheckAsync(RequestModel model) =>
            Publish<EmailScanProfile>(model);
    }
}