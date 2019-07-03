using System.Reflection;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;

namespace Falcon.Bus.EasyNetQ
{
    public interface IBusSubscriber
    {
        void Subscribe();
    }

    public class BusSubscriber : IBusSubscriber
    {
        private readonly IBus _bus;
        private readonly IAutoSubscriberMessageDispatcher _messageDispatcher;

        public BusSubscriber(IBus bus, IAutoSubscriberMessageDispatcher messageDispatcher)
        {
            _bus = bus;
            _messageDispatcher = messageDispatcher;
        }

        public void Subscribe()
        {
            var subscriber = new AutoSubscriber(_bus, "_")
            {
                ConfigureSubscriptionConfiguration = s => s.WithAutoDelete(),
                GenerateSubscriptionId = info => "_",
                AutoSubscriberMessageDispatcher = _messageDispatcher
            };

            subscriber.SubscribeAsync(Assembly.GetEntryAssembly());
        }
    }
}