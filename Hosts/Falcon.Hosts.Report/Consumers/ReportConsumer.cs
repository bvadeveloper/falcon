using System;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Profiles.Report;

namespace Falcon.Hosts.Report.Consumers
{
    public class ReportConsumer : IConsumeAsync<ReportProfile>
    {
        public Task ConsumeAsync(ReportProfile message)
        {
            message.Reports.ForEach(model => Console.WriteLine(model.Output));

            return Task.CompletedTask;
        }
    }
}