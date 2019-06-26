using Autofac;
using EasyNetQ;
using Util.Extensions;

namespace Falcon.EasyNetQ.Module
{
    public class EasyNetQModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModel<ConnectionConfiguration>("RabbitMQ");

            builder.Register(c =>
            {
                var configuration = c.Resolve<ConnectionConfiguration>();
                return RabbitHutch.CreateBus(configuration, null);
            }).As<IBus>();
        }
    }
}