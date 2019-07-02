using System;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Profiles.Collect;

namespace Falcon.Hosts.Сollector.Consumers
{
    public class CollectorConsumer : IConsumeAsync<CollectDomainProfile>
    {
        public Task ConsumeAsync(CollectDomainProfile message)
        {
            Console.WriteLine(message.Targets.FirstOrDefault());

            return Task.CompletedTask;
        }
    }
}