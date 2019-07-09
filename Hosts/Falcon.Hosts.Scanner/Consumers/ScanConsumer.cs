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

        private static string MakeScanReportKey(string name) => $"scan:{name}";

        public ScanConsumer(IBus bus,
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
            var reports = await ScanTargetByProfile(profile);
            await PublishReportProfile(profile, reports);
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

        private async Task<List<ReportModel>> ScanTargetByProfile(DomainScanProfile profile)
        {
            var scanReportsCache = await _cacheService.GetValueAsync<List<ReportModel>>(MakeScanReportKey(profile.Target));

            if (scanReportsCache == null)
            {
                var outputs = await _toolsFactory(ToolType.Scan)
                    .UseOptionalTools(profile.Tools) // use tools from client request
                    .MapToolsByTags(profile.Tags) // if tools are empty map tools by tags
                    .InitTools()
                    .RunToolsAsync(profile.Target);

                _logger.LogOutputs(outputs);

                var scanReports = outputs
                    .GetSuccessful()
                    .Select(f => new ReportModel
                        { ToolName = f.ToolName, Output = f.Output, ProcessingDate = DateTime.UtcNow })
                    .ToList();

                await _cacheService.SetValueAsync(MakeScanReportKey(profile.Target), scanReports, _ttl);

                return scanReports;
            }

            return scanReportsCache;
        }
    }
}