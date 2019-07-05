using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Falcon.Logging;
using Falcon.Profiles.Collect;
using Falcon.Tools;

namespace Falcon.Hosts.Сollector.Consumers
{
    public class CollectorConsumer : IConsumeAsync<DomainCollectProfile>
    {
        private readonly IBus _bus;
        private readonly ToolsHolder.Factory _tooFactory;
        private readonly IJsonLogger _logger;

        public CollectorConsumer(
            IBus bus,
            ToolsHolder.Factory tooFactory,
            IJsonLogger<CollectorConsumer> logger)
        {
            _bus = bus;
            _tooFactory = tooFactory;
            _logger = logger;
        }

        public async Task ConsumeAsync(DomainCollectProfile message)
        {
            var r = await _tooFactory(ToolType.Collect)
                .MakeTools()
                .RunToolsAsync(message.Target);

            _logger.Information("wwwwwwwwwwww");

//            var result = await _tooFactory
//                .Invoke(message.Target, ToolType.Collect)
//                .RunAsync();


//            foreach (var d in result)
//            {
//                _logger.Information(d);
//            }

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