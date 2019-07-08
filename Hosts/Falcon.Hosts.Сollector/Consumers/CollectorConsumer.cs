using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Profiles.Collect;
using Falcon.Tools;
using Falcon.Tools.Interfaces;

namespace Falcon.Hosts.Сollector.Consumers
{
    public class CollectorConsumer : IConsumeAsync<DomainCollectProfile>
    {
        private readonly IBus _bus;
        private readonly TagFactory.Factory _tagService;
        private readonly ToolsHolder.Factory _toolsFactory;
        private readonly IJsonLogger _logger;

        public CollectorConsumer(
            IBus bus,
            ToolsHolder.Factory toolsFactory,
            TagFactory.Factory tagService,
            IJsonLogger<CollectorConsumer> logger)
        {
            _bus = bus;
            _toolsFactory = toolsFactory;
            _tagService = tagService;
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
                // 2. send scan profile to scanners
                // 3. send request to save target attributes to DB
                // 4. save data to Redis- ttl 1 hour
                // 5. send target attributes to report for sending to clients
            }


            var outputs = await _toolsFactory(ToolType.Collect)
                .MakeTools()
                .RunToolsAsync(message.Target);

            _logger.LogOutputs(outputs);

            var successfulOutputs = outputs.GetSuccessful();
            var targetTags = _tagService.FindTags(successfulOutputs);


            // rs.ToList().ForEach(r => _logger.Trace(r.Output));

//            if (!data.Any())
//            {
//                // send report and stop
//                await _bus.PublishAsync(new CollectReport { Report = $"'{message.Target}' can't be found" });
//                return;
//            }
//
//            // save collected data to db
//            await _bus.PublishAsync(new SaveProfile { Data = data.FirstOrDefault() });
//
//            // send messages to run scanners
//            await _bus.PublishAsync(new DomainScanProfile
//            {
//                Target = message.Target,
//                Tools = message.Tools.Any() ? message.Tools : _toolService.PickupTools(data),
//                TargetData = new Dictionary<TargetAttributes, string>()
//            });
//
//            // send report 
//            await _bus.PublishAsync(new CollectReport { Report = $"data for target '{data}'" });
        }
    }
}