using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Profiles.Telegram;
using Falcon.Reports;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Falcon.Hosts.Telegram.Consumers
{
    public class TelegramTextConsumer : IConsumeAsync<TelegramTextProfile>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IJsonLogger _logger;

        /// <summary>
        /// Telegram message length
        /// </summary>
        private const int MessageLength = 4095;

        public TelegramTextConsumer(
            ITelegramBotClient botClient,
            IJsonLogger<TelegramFileConsumer> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        public async Task ConsumeAsync(TelegramTextProfile profile)
        {
            var context = profile.Context as MessengerContext;

            if (context == null)
            {
                _logger.Error($"Profile context can't be cast to '{typeof(MessengerContext).Name}'");
                return;
            }

            await _botClient.SendChatActionAsync(context.ChatId, ChatAction.Typing);
            await SendTextAsync(context.ChatId, profile.Reports);
        }

        /// <summary>
        /// Send text to chat
        /// </summary>
        private async Task SendTextAsync(long chatId, IEnumerable<ReportModel> reports)
        {
            foreach (var report in reports)
            {
                foreach (var spl in report.Output.SplitBy(MessageLength))
                {
                    await _botClient.SendTextMessageAsync(chatId, spl);
                }
            }
        }
    }

    public static class EnumerableExtension
    {
        /// <summary>
        /// Split string by chunk length
        /// </summary>
        public static IEnumerable<string> SplitBy(this string @string, int chunkLength)
        {
            if (string.IsNullOrEmpty(@string))
            {
                throw new ArgumentException($"Param '{nameof(@string)}' can't be null or empty");
            }

            if (chunkLength < 1)
            {
                throw new ArgumentException($"Param '{nameof(chunkLength)}' can't be less than 1");
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