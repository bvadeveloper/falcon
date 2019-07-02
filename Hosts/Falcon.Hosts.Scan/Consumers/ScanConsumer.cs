using System;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Profiles.Scan;

namespace Falcon.Hosts.Scan.Consumers
{
    public class ScanConsumer : IConsumeAsync<ScanDomainProfile>
    {
        public Task ConsumeAsync(ScanDomainProfile message)
        {
            Console.WriteLine(message.Targets.FirstOrDefault());

            return Task.CompletedTask;
        }
    }
}