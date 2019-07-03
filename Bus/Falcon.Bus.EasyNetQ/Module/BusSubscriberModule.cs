using System.Reflection;
using Autofac;
using EasyNetQ.AutoSubscribe;

namespace Falcon.Bus.EasyNetQ.Module
{
    public class BusSubscriberModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<BusModule>();
            builder.RegisterType<MessageDispatcher>().As<IAutoSubscriberMessageDispatcher>();
            builder.RegisterType<BusSubscriber>().As<IBusSubscriber>();
            
            // register consumers with autofac, EasyNetQ has some tricky ways to use autocustomers
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                .Where(t => t.Name.EndsWith("Consumer")).AsSelf();
        }
    }
}