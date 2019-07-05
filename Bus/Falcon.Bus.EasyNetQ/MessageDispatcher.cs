using System;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Falcon.Bus.EasyNetQ
{
    public class MessageDispatcher : IAutoSubscriberMessageDispatcher
    {
        private readonly IServiceProvider _provider;
        private readonly IJsonLogger _logger;

        public MessageDispatcher(IServiceProvider provider, IJsonLogger<MessageDispatcher> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        public void Dispatch<TMessage, TConsumer>(TMessage message)
            where TMessage : class
            where TConsumer : class, IConsume<TMessage>
        {
            using (var scope = _provider.CreateScope())
            {
                try
                {
                    var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();
                    consumer.Consume(message);
                }
                catch (InvalidOperationException operationException)
                {
                    _logger.Error($"Can't resolve consumer '{typeof(TConsumer).Name}' for message '{typeof(TMessage)}'",
                        message, operationException);
                }
                catch (Exception ex)
                {
                    _logger.Error("Processing failed", message, ex);
                }
            }
        }

        public async Task DispatchAsync<TMessage, TConsumer>(TMessage message)
            where TMessage : class
            where TConsumer : class, IConsumeAsync<TMessage>
        {
            using (var scope = _provider.CreateScope())
            {
                try
                {
                    var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();
                    await consumer.ConsumeAsync(message);
                }
                catch (InvalidOperationException operationException)
                {
                    _logger.Error($"Can't resolve consumer '{typeof(TConsumer).Name}' for message '{typeof(TMessage)}'",
                        message, operationException);
                }
                catch (Exception ex)
                {
                    _logger.Error("Processing failed", message, ex);
                }
            }
        }
    }
}