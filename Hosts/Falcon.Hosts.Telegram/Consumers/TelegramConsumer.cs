using System;
using System.Collections.Generic;
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

            foreach (var report in profile.Reports)
            {
                foreach (var spl in report.Output.SplitBy(4095))
                {
                    await _botClient.SendTextMessageAsync(prof.ChatId, spl);
                }
            }
        }
    }

    public static class EnumerableEx
    {
        public static IEnumerable<string> SplitBy(this string str, int chunkLength)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentException();
            if (chunkLength < 1) throw new ArgumentException();

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }
    }
}