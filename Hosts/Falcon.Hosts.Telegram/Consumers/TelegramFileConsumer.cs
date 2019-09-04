using System.IO;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Profiles.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Falcon.Hosts.Telegram.Consumers
{
    public class TelegramFileConsumer : IConsumeAsync<TelegramFileProfile>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IJsonLogger _logger;

        /// <summary>
        /// Telegram message length
        /// </summary>
        private const int MessageLength = 4095;

        public TelegramFileConsumer(
            ITelegramBotClient botClient,
            IJsonLogger<TelegramFileConsumer> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        public async Task ConsumeAsync(TelegramFileProfile profile)
        {
            var context = profile.Context as MessengerContext;

            if (context == null)
            {
                _logger.Error($"Profile context can't be cast to '{typeof(MessengerContext).Name}'");
                return;
            }

            await _botClient.SendChatActionAsync(context.ChatId, ChatAction.Typing);
            await SendFileAsync(context.ChatId, profile.File, profile.FileName);
        }

        /// <summary>
        /// Send file to chat
        /// </summary>
        private async Task SendFileAsync(long chatId, byte[] bytes, string fileName)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var inputMedia = new InputMedia(stream, fileName);
                await _botClient.SendDocumentAsync(chatId, inputMedia);
            }
        }
    }
}