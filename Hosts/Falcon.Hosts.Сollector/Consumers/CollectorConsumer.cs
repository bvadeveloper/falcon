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

        private static string MakeTagKey(string name) => $"tags:{name}";

        private static string MakeCollectReportKey(string name) => $"collect:{name}";

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

                // 2. start collect tools, fill target tags
                var (collectReports, tags) = await CollectInfoByProfile(profile);

                // 3. send request to save target tags to DB
                await PublishSaveProfile(profile, tags);

                // 4. send target tags to the report host to send to clients
              //  await PublishReportProfile(profile, collectReports);
            }
            else
            {
                // 1. start collect tools, fill target tags
                var (collectReports, tags) = await CollectInfoByProfile(profile);

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
                    await PublishSaveProfile(profile, tags);

                    // 4. send target tags to the report host to send to clients
                  //  await PublishReportProfile(profile, collectReports);
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

        private async Task<(List<ReportModel>, Dictionary<TagType, string>)> CollectInfoByProfile(
            DomainCollectProfile profile)
        {
            var collectReportsCache =
                await _cacheService.GetValueAsync<List<ReportModel>>(MakeCollectReportKey(profile.Target));

            if (collectReportsCache == null)
            {
                var outputs = await _toolsFactory(ToolType.Collect)
                    .UseTools(profile.Tools)
                    .RunToolsAsync(profile.Target);

                _logger.LogOutputs(outputs);

                var collectReports = outputs
                    .GetSuccessful()
                    .Select(f => new ReportModel
                        { ToolName = f.ToolName, Output = f.Output, ProcessingDate = DateTime.UtcNow })
                    .ToList();

                var tags = await GetTags(profile.Target, collectReports);

                await _cacheService.SetValueAsync(MakeTagKey(profile.Target), tags, _ttl);
                await _cacheService.SetValueAsync(MakeCollectReportKey(profile.Target), collectReports, _ttl);

                return (collectReports, tags);
            }

            return (collectReportsCache, await GetTags(profile.Target, collectReportsCache));
        }

        private async Task<Dictionary<TagType, string>> GetTags(string target,
            IEnumerable<ReportModel> collectReportsCache)
        {
            return await _cacheService.GetValueAsync<Dictionary<TagType, string>>(MakeTagKey(target)) ??
                   _tagService.FindTags(collectReportsCache);
        }
    }
}