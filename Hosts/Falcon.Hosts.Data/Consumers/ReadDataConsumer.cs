using System;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Profiles.Data;

namespace Falcon.Hosts.Data.Consumers
{
    public class ReadDataConsumer : IConsumeAsync<ReadProfile>
    {
        public Task ConsumeAsync(ReadProfile message)
        {
            Console.WriteLine(message.Request);

            return Task.CompletedTask;
        }
    }
}