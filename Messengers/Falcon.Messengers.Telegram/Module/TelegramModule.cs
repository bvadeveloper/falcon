using Autofac;
using Falcon.Messengers.Telegram.Configuration;
using Falcon.Utils.Autofac;
using Telegram.Bot;

namespace Falcon.Messengers.Telegram.Module
{
    public class TelegramModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModel<MessengerConfiguration>("Telegram");

            builder.Register(c =>
            {
                var config = c.Resolve<MessengerConfiguration>();
                return new TelegramBotClient(config.ApiKey);
            }).As<ITelegramBotClient>();

            builder.RegisterType<TelegramMessageHandler>().As<IMessageHandler>();
        }
    }
}