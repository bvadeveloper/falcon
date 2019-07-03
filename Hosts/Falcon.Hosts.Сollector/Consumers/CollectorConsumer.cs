using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Profiles.Collect;
using Falcon.Profiles.Data;
using Falcon.Profiles.Scan;
using Falcon.Reports;
using Falcon.Services.Tool;
using Falcon.Tools;

namespace Falcon.Hosts.Сollector.Consumers
{
    public class CollectorConsumer : IConsumeAsync<DomainCollectProfile>
    {
        private readonly IBus _bus;
        private readonly IJsonLogger _logger;
        private readonly IToolService _toolService;

        public CollectorConsumer(IBus bus, IJsonLogger<CollectorConsumer> logger, IToolService toolService)
        {
            _bus = bus;
            _logger = logger;
            _toolService = toolService;
        }

        public async Task ConsumeAsync(DomainCollectProfile message)
        {
            var data = await ToolFactory
                .Init()
                .AddTarget(message.Target)
                .UseCollectTools()
                .RunAsync();

            foreach (var d in data)
            {
                _logger.Information(d);
            }

            if (!data.Any())
            {
                // send report and stop
                await _bus.PublishAsync(new CollectReport { Report = $"'{message.Target}' can't be found" });
                return;
            }

            // save collected data to db
            await _bus.PublishAsync(new SaveProfile { Data = data.FirstOrDefault() });

            // send messages to run scanners
            await _bus.PublishAsync(new DomainScanProfile
            {
                Target = message.Target,
                Tools = message.Tools.Any() ? message.Tools : _toolService.PickupTools(data),
                TargetData = new Dictionary<TargetAttributes, string>()
            });

            // send report 
            await _bus.PublishAsync(new CollectReport { Report = $"data for target '{data}'" });
        }
    }
}