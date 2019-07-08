using System;
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
                await _bus.PublishAsync(new DomainScanProfile
                {
                    Context = profile.Context,
                    Target = profile.Target,
                    Tools = profile.Tools
                });

                // 2. check if target attributes already in cache skip steps #3, #4, #5
                var tagsCache = await _cacheService.GetValueAsync(MakeTagKey(profile.Target));

                if (tagsCache == null)
                {
                    // 3. start collect tools, fill target attributes
                    // 4. send request to save target attributes to DB
                    // 5. save data to Redis - ttl 1 hour 
                    // 6. send target attributes to report for sending to clients
                }
                else
                {
                    // 6. send target tags to report for sending to clients
                }
            }
            else
            {
                // no any tools

                // 1. start collect tools, fill target attributes
                var outputs = await _toolsFactory(ToolType.Collect)
                    .MakeTools()
                    .RunToolsAsync(profile.Target);

                _logger.LogOutputs(outputs);

                var collectReports = outputs
                    .GetSuccessful()
                    .Select(f => new ReportModel { ToolName = f.ToolName, Output = f.Output })
                    .ToList();

                var tags = _tagService.FindTags(collectReports);

                if (tags.ContainsKey(TargetTag.Alive))
                {
                    // target is available


                    // 2. send scan profile to scanners
                    await _bus.PublishAsync(new DomainScanProfile
                    {
                        Context = profile.Context,
                        Target = profile.Target,
                        Tags = tags
                    });

                    // 3. send request to save target tags to DB
                    await _bus.PublishAsync(new SaveProfile
                    {
                        Context = profile.Context,
                        ScanDate = DateTime.UtcNow,
                        Tags = tags
                    });

                    // 4. save data to Redis - ttl 1 hour
                    await _cacheService.SetValueAsync(MakeTagKey(profile.Target), tags, TimeSpan.FromHours(1));
                    await _cacheService.SetValueAsync(MakeCollectReportKey(profile.Target), collectReports,
                        TimeSpan.FromHours(1));

                    // 5. send target attributes to report for sending to clients
                    await _bus.PublishAsync(new CollectReportProfile
                    {
                        Context = profile.Context,
                        Target = profile.Target,
                        ReportModels = collectReports
                    });
                }
                else
                {
                    // target is NOT available

                    // 5. send target tags to report host for sending to clients
                    await _bus.PublishAsync(new CollectReportProfile
                    {
                        Context = profile.Context,
                        Target = profile.Target,
                        ReportModels = collectReports
                    });
                }
            }
        }
    }
}