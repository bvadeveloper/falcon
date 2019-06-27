using Autofac;
using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace Falcon.Bus.EasyNetQ.Module
{
    public class EasyNetQModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var configuration = c.Resolve<IConfiguration>();
                var connectionString = configuration.GetConnectionString("RabbitMQ");
                return RabbitHutch.CreateBus(connectionString);
            }).As<IBus>();
        }
    }
}