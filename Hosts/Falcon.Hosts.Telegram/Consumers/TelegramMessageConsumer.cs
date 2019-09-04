using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Profiles.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Falcon.Hosts.Telegram.Consumers
{
    public class TelegramMessageConsumer : IConsumeAsync<TelegramMessageProfile>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IJsonLogger _logger;

        /// <summary>
        /// Telegram message length
        /// </summary>
        private const int MessageLength = 4095;

        public TelegramMessageConsumer(
            ITelegramBotClient botClient,
            IJsonLogger<TelegramFileConsumer> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        public async Task ConsumeAsync(TelegramMessageProfile profile)
        {
            var context = profile.Context as MessengerContext;

            if (context == null)
            {
                _logger.Error($"Profile context can't be cast to '{typeof(MessengerContext).Name}'");
                return;
            }

            await SendMessageAsync(context.ChatId, profile.Message);
        }

        /// <summary>
        /// Send text to chat
        /// </summary>
        private async Task SendMessageAsync(long chatId, string report)
        {
            foreach (var spl in report.SplitBy(MessageLength))
            {
                await _botClient.SendTextMessageAsync(chatId, spl);
            }
        }
    }

    public static class StringExtension
    {
        /// <summary>
        /// Split string by chunk length
        /// </summary>
        public static IEnumerable<string> SplitBy(this string @string, int chunkLength)
        {
            if (string.IsNullOrEmpty(@string))
            {
                throw new ArgumentException($"'{nameof(@string)}' can't be null or empty");
            }

            if (chunkLength < 1)
            {
                throw new ArgumentException($"'{nameof(chunkLength)}' can't be less than 1");
            }

            for (var i = 0; i < @string.Length; i += chunkLength)
            {
                if (chunkLength + i > @string.Length)
                {
                    chunkLength = @string.Length - i;
                }

                yield return @string.Substring(i, chunkLength);
            }
        }
    }
}