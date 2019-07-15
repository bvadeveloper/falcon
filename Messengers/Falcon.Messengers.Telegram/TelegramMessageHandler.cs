using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Services.RequestManagement;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Falcon.Messengers.Telegram
{
    public class TelegramMessageHandler : IMessageHandler
    {
        private readonly IRequestManagementService _processingService;
        private readonly ITelegramBotClient _botClient;
        private readonly IMessengerContext _sessionContext;
        private readonly IJsonLogger _logger;

        public TelegramMessageHandler(
            IRequestManagementService processingService,
            ITelegramBotClient botClient,
            IJsonLogger<TelegramMessageHandler> logger,
            IMessengerContext sessionContext)
        {
            _processingService = processingService;
            _botClient = botClient;
            _sessionContext = sessionContext;
            _logger = logger;
        }

        public void SubscribeOnBot(ITelegramBotClient botClient)
        {
            botClient.OnMessage += Message;
            botClient.OnUpdate += Update;
            botClient.OnMessageEdited += Edit;
            botClient.OnCallbackQuery += Callback;
            botClient.OnInlineQuery += Inline;
            botClient.OnInlineResultChosen += InlineResult;
            botClient.OnReceiveError += Error;
        }

        private InlineKeyboardMarkup MakeReportButtons()
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new[] // first row
                {
                    new InlineKeyboardButton { Text = "text", CallbackData = "text" },
                    new InlineKeyboardButton { Text = "pdf", CallbackData = "pdf" },
                },
            });
        }

        public async void Message(object sender, MessageEventArgs e)
        {
            var message = e.Message;

            if (message == null || message.Type != MessageType.Text)
            {
                return;
            }

            var text = message.Text.Trim().Split(' ').FirstOrDefault();

            switch (text)
            {
                case "/inline":
                    await _botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "report format?",
                        replyMarkup: MakeReportButtons());
                    break;
                default:
                    await ProcessRequest(text, message.Chat.Id, message.Chat.Username);
                    break;
            }
        }

        private async Task ProcessRequest(string text, long chatId, string userName)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                await _botClient.SendTextMessageAsync(chatId, "input not recognized");
                return;
            }

            await _botClient.SendChatActionAsync(chatId, ChatAction.Typing);

            _sessionContext.ChatId = chatId;
            _sessionContext.ClientName = userName;

            var result = await _processingService.DomainsVulnerabilityScanAsync(new RequestModel
                { Targets = new List<string> { text } });

            await _botClient.SendTextMessageAsync(chatId, result.Value);
        }

        public void Error(object sender, ReceiveErrorEventArgs e)
        {
            _logger.Error($"Received error: {e.ApiRequestException.ErrorCode} — {e.ApiRequestException.Message}");
        }

        public void Edit(object sender, MessageEventArgs e)
        {
            _logger.Information($"Received: Edit");
        }

        public void Callback(object sender, CallbackQueryEventArgs e)
        {
            _logger.Information($"Received: Callback");
        }

        public void Inline(object sender, InlineQueryEventArgs e)
        {
            _logger.Information($"Received: Inline");
        }

        public void InlineResult(object sender, ChosenInlineResultEventArgs e)
        {
            _logger.Information($"Received: InlineResult");
        }

        public void Update(object sender, UpdateEventArgs e)
        {
            _logger.Information($"Received: Update");
        }
    }
}