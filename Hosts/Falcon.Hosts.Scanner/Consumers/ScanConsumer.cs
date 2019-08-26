using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Falcon.Data.Redis;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Profiles.Report;
using Falcon.Profiles.Scan;
using Falcon.Tools;

namespace Falcon.Hosts.Scanner.Consumers
{
    public class ScanConsumer : IConsumeAsync<DomainScanProfile>
    {
        private readonly IBus _bus;
        private readonly ToolsFactory.Factory _toolsFactory;
        private readonly ICacheService _cacheService;
        private readonly IJsonLogger _logger;

        private readonly TimeSpan _ttl = TimeSpan.FromHours(1);

        private static string MakeReportKey(string name) => $"scan:{name}";

        public ScanConsumer(
            IBus bus,
            ToolsFactory.Factory toolsFactory,
            ICacheService cacheService,
            IJsonLogger<ScanConsumer> logger)
        {
            _bus = bus;
            _toolsFactory = toolsFactory;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task ConsumeAsync(DomainScanProfile profile)
        {
            var result = await ScanTargetAsync(profile);
            await PublishReportProfile(profile, result);
        }

        private async Task PublishReportProfile(ITargetProfile profile, List<ReportModel> scanReports)
        {
            await _bus.PublishAsync(new ReportProfile
            {
                Context = profile.Context,
                Target = profile.Target,
                Reports = scanReports,
            });
        }

        private async Task<List<ReportModel>> ScanTargetAsync(DomainScanProfile profile)
        {
            var reports = await _cacheService.GetValueAsync<List<ReportModel>>(MakeReportKey(profile.Target));

            if (reports == null)
            {
                var outputs = await _toolsFactory(ToolType.Scan)
                    .UseToolsByTags(profile.Tags) // map tool from tags if empty or use from profile
                    .RunToolsAsync(profile.Target);

                _logger.LogOutputs(outputs);

                reports = outputs
                    .SelectSuccessful()
                    .Select(f => new ReportModel
                        {ToolName = f.ToolName, Output = f.Output, ProcessingDate = DateTime.UtcNow})
                    .ToList();

                await _cacheService.SetValueAsync(MakeReportKey(profile.Target), reports, _ttl);

                return reports;
            }

            return reports;
        }
    }
}