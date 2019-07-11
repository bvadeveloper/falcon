using System.Linq;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Profiles;
using Falcon.Profiles.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Falcon.Hosts.Telegram.Consumers
{
    public class TelegramConsumer : IConsumeAsync<TelegramProfile>
    {
        private readonly ITelegramBotClient _botClient;

        public TelegramConsumer(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task ConsumeAsync(TelegramProfile profile)
        {
            var prof = profile.Context as MessengerContext;

            await _botClient.SendChatActionAsync(prof.ChatId, ChatAction.Typing);
            await _botClient.SendTextMessageAsync(prof.ChatId, profile.Reports.FirstOrDefault().Output);
        }
    }
}