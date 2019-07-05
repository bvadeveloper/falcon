using System;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Profiles.Scan;

namespace Falcon.Hosts.Scanner.Consumers
{
    public class ScanConsumer : IConsumeAsync<DomainScanProfile>
    {
        public Task ConsumeAsync(DomainScanProfile message)
        {
            Console.WriteLine(message.Target.FirstOrDefault());

            return Task.CompletedTask;
        }
    }
}