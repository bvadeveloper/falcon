using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Falcon.Data.Redis;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Profiles.Collect;
using Falcon.Profiles.Data;
using Falcon.Profiles.Report;
using Falcon.Profiles.Scan;
using Falcon.Tools;

namespace Falcon.Hosts.Сollector.Consumers
{
    public class CollectorConsumer : IConsumeAsync<DomainCollectProfile>
    {
        private readonly IBus _bus;
        private readonly TagFactory.Factory _tagService;
        private readonly ToolsFactory.Factory _toolsFactory;
        private readonly ICacheService _cacheService;
        private readonly IJsonLogger _logger;

        private readonly TimeSpan _ttl = TimeSpan.FromHours(1);

        private static string MakeReportKey(string name) => $"collect:{name}";

        public CollectorConsumer(
            IBus bus,
            ToolsFactory.Factory toolsFactory,
            TagFactory.Factory tagService,
            ICacheService cacheService,
            IJsonLogger<CollectorConsumer> logger)
        {
            _bus = bus;
            _toolsFactory = toolsFactory;
            _tagService = tagService;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task ConsumeAsync(DomainCollectProfile profile)
        {
            if (profile.HasTools())
            {
                // 1. send scan profile to scanners
                await PublishScanProfile(profile);
            }
            else
            {
                // 1. start collect tools, fill target tags
                var (collectReports, tags) = await CollectTagsAsync(profile);

                if (tags.ContainsKey(TagType.NotAvailable))
                {
                    // target is NOT available

                    // 4. send target tags to report host for sending to clients
                    await PublishReportProfile(profile, collectReports);
                }
                else
                {
                    // target is available

                    // 2. send scan profile to scanners
                    await PublishScanProfile(profile, tags);

                    // 3. send request to save target tags to DB
                    // await PublishSaveProfile(profile, tags);
                }
            }
        }

        private async Task PublishScanProfile(DomainCollectProfile profile,
            Dictionary<TagType, string> tags = default)
        {
            await _bus.PublishAsync(new DomainScanProfile
            {
                Context = profile.Context,
                Target = profile.Target,
                Tools = profile.Tools,
                Tags = tags
            });
        }

        private async Task PublishSaveProfile(ISession profile,
            Dictionary<TagType, string> tags)
        {
            await _bus.PublishAsync(new SaveProfile
            {
                Context = profile.Context,
                ScanDate = DateTime.UtcNow,
                Tags = tags
            });
        }

        private async Task PublishReportProfile(ITargetProfile profile, List<ReportModel> collectReports)
        {
            await _bus.PublishAsync(new ReportProfile
            {
                Context = profile.Context,
                Target = profile.Target,
                Reports = collectReports
            });
        }

        private async Task<(List<ReportModel>, Dictionary<TagType, string>)> CollectTagsAsync(DomainCollectProfile profile)
        {
            var reports = await _cacheService.GetValueAsync<List<ReportModel>>(MakeReportKey(profile.Target));

            if (reports == null)
            {
                var outputs = await _toolsFactory(ToolType.Collect)
                    .UseTools()
                    .RunToolsAsync(profile.Target);

                _logger.LogOutputs(outputs);

                reports = outputs.SelectSuccessful()
                    .Select(f => new ReportModel {ToolName = f.ToolName, Output = f.Output, ProcessingDate = DateTime.UtcNow})
                    .ToList();

                await _cacheService.SetValueAsync(MakeReportKey(profile.Target), reports, _ttl);

                return (reports, _tagService.FindTags(reports));
            }

            return (reports, _tagService.FindTags(reports));
        }
    }
}