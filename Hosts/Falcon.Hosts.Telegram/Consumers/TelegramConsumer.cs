using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Profiles.Telegram;
using Telegram.Bot;

namespace Falcon.Hosts.Telegram.Consumers
{
    public class TelegramConsumer : IConsumeAsync<TelegramProfile>
    {
        private readonly ITelegramBotClient _botClient;

        public TelegramConsumer(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task ConsumeAsync(TelegramProfile profile) { }
    }
}