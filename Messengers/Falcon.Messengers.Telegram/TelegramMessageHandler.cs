using System.Collections.Generic;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Services.RequestManagement;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace Falcon.Messengers.Telegram
{
    public class TelegramMessageHandler : IMessageHandler
    {
        private readonly IRequestManagementService _processingService;
        private readonly ITelegramBotClient _botClient;
        private readonly IJsonLogger _logger;

        public TelegramMessageHandler(
            IRequestManagementService processingService,
            ITelegramBotClient botClient,
            IJsonLogger<TelegramMessageHandler> logger)
        {
            _processingService = processingService;
            _botClient = botClient;
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

        public async void Message(object sender, MessageEventArgs e)
        {
            var message = e.Message;

            if (message == null || message.Type != MessageType.Text)
            {
                return;
            }

            await _botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            var result = await _processingService.DomainsVulnerabilityScanAsync(new RequestModel
                { Targets = new List<string> { "message" } });

            await _botClient.SendTextMessageAsync(message.Chat.Id, result.Value);
        }

        public void Error(object sender, ReceiveErrorEventArgs e)
        {
            _logger.Error($"Received error: {e.ApiRequestException.ErrorCode} — {e.ApiRequestException.Message}");
        }

        public void Edit(object sender, MessageEventArgs e)
        {
            _logger.Information($"Received: {e.Message.Caption}");
        }

        public void Callback(object sender, CallbackQueryEventArgs e)
        {
            _logger.Information($"Received: {e.CallbackQuery.Message.Caption}");
        }

        public void Inline(object sender, InlineQueryEventArgs e)
        {
            _logger.Information($"Received: {e.InlineQuery.Query}");
        }

        public void InlineResult(object sender, ChosenInlineResultEventArgs e)
        {
            _logger.Information($"Received: {e.ChosenInlineResult.Query}");
        }

        public void Update(object sender, UpdateEventArgs e)
        {
            _logger.Information($"Received: {e.Update.Message.Caption}");
        }
    }
}