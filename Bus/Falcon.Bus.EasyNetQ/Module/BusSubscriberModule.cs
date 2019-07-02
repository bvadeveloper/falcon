using System.Reflection;
using Autofac;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.Configuration;

namespace Falcon.Bus.EasyNetQ.Module
{
    public class BusSubscriberModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var configuration = c.Resolve<IConfiguration>();
                var connectionString = configuration.GetConnectionString("RabbitMQ");
                var bus = RabbitHutch.CreateBus(connectionString);
                var subscriber = new AutoSubscriber(bus, "host")
                {
                    ConfigureSubscriptionConfiguration = s => s.WithAutoDelete(),
                    GenerateSubscriptionId = info => "host"
                };
                subscriber.SubscribeAsync(Assembly.GetEntryAssembly());

                return bus;
            }).As<IBus>();
        }
    }
}