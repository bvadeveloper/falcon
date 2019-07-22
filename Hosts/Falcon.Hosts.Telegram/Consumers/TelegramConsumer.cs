using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Profiles.Telegram;
using Falcon.Reports;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Falcon.Hosts.Telegram.Consumers
{
    public class TelegramConsumer : IConsumeAsync<TelegramProfile>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IJsonLogger _logger;

        /// <summary>
        /// Telegram message length
        /// </summary>
        private const int MessageLength = 4095;

        public TelegramConsumer(
            ITelegramBotClient botClient,
            IJsonLogger<TelegramConsumer> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        public async Task ConsumeAsync(TelegramProfile profile)
        {
            var context = profile.Context as MessengerContext;

            if (context == null)
            {
                _logger.Error($"Profile context can't be cast to '{typeof(MessengerContext).Name}'");
                return;
            }

            await _botClient.SendChatActionAsync(context.ChatId, ChatAction.Typing);

            switch (context.ReportType)
            {
                case ReportType.Text:
                    await SendTextAsync(context.ChatId, profile.Reports);
                    break;
                case ReportType.Pdf:
                    await SendPdfAsync(context.ChatId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Send pdf to chat
        /// </summary>
        private async Task SendPdfAsync(long chatId)
        {
            var bytes = Encoding.Default.GetBytes("pdf data file");
            var stream = new MemoryStream(bytes);
            var file = new InputMedia(stream, "report.pdf");

            await _botClient.SendDocumentAsync(chatId, file);
        }

        /// <summary>
        /// Send text to chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="reports"></param>
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
        /// <param name="string"></param>
        /// <param name="chunkLength"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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