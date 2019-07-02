using System;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Reports;

namespace Falcon.Hosts.Report.Consumers
{
    public class ScanReportConsumer : IConsumeAsync<ScanReport>
    {
        public Task ConsumeAsync(ScanReport message)
        {
            Console.WriteLine(message.Report);

            return Task.CompletedTask;
        }
    }
}