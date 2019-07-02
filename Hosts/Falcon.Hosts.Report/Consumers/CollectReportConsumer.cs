using System;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Reports;

namespace Falcon.Hosts.Report.Consumers
{
    public class CollectReportConsumer : IConsumeAsync<CollectReport>
    {
        public Task ConsumeAsync(CollectReport message)
        {
            Console.WriteLine(message.Report);

            return Task.CompletedTask;
        }
    }
}