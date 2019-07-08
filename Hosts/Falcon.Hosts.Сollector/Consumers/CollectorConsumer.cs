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

        public async Task ConsumeAsync(DomainCollectProfile message)
        {
            if (message.HasTools())
            {
                // 1. send scan profile to scanners
                // 2. check if target attributes already in cache skip steps #3, #4, #5
                // 3. start collect tools, fill target attributes
                // 4. send request to save target attributes to DB
                // 5. save data to Redis - ttl 1 hour 
                // 6. send target attributes to report for sending to clients
            }
            else
            {
                // 1. start collect tools, fill target attributes
                var outputs = await _toolsFactory(ToolType.Collect)
                    .MakeTools()
                    .RunToolsAsync(message.Target);

                _logger.LogOutputs(outputs);

                var successfulOutputs = outputs.GetSuccessful();
                var tags = _tagService.FindTags(successfulOutputs);

                if (tags.ContainsKey(TargetTag.Alive))
                {
                    // 2. send scan profile to scanners
                    await _bus.PublishAsync(new DomainScanProfile
                    {
                        Context = message.Context,
                        Target = message.Target,
                        Tags = tags
                    });

                    // 3. send request to save target attributes to DB
                    await _bus.PublishAsync(new SaveProfile
                    {
                        Context = message.Context,
                        ScanDate = DateTime.UtcNow,
                        Tags = tags
                    });

                    // 4. save data to Redis- ttl 1 hour

                    // 5. send target attributes to report for sending to clients
                    await _bus.PublishAsync(new CollectReportProfile
                    {
                        Context = message.Context,
                        Target = message.Target,
                        ReportModels = successfulOutputs.Select(f => new ReportModel
                            { ToolName = f.ToolName, Output = f.Output }).ToList()
                    });
                }
                else
                {
                    // target not available
                    
                    // 5. send target attributes to report for sending to clients
                    await _bus.PublishAsync(new CollectReportProfile
                    {
                        Context = message.Context,
                        Target = message.Target,
                        ReportModels = successfulOutputs.Select(f => new ReportModel
                            { ToolName = f.ToolName, Output = f.Output }).ToList()
                    });
                }
            }
        }
    }
}