using System;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Profiles.Collect;

namespace Falcon.Hosts.Сollector.Consumers
{
    public class CollectorConsumer : IConsumeAsync<DomainCollectProfile>
    {
        public Task ConsumeAsync(DomainCollectProfile message)
        {
            Console.WriteLine(message.Target);

            return Task.CompletedTask;
        }
    }
}