using System;
using System.Linq;
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

        public void Subscribe(ITelegramBotClient botClient)
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

            if (message.Type == MessageType.Text)
            {
                var targets = message.Text.Trim().Split(Environment.NewLine).Where(d => !string.IsNullOrWhiteSpace(d)).ToList();

                if (targets.Any())
                {
                    SetSessionContext(message.Chat.Id, message.Chat.Username);
                    var confirmation = await _processingService.DomainsVulnerabilityScanAsync(new RequestModel {Targets = targets});
                    await _botClient.SendTextMessageAsync(message.Chat.Id, confirmation.Value);
                }
                else
                {
                    await _botClient.SendTextMessageAsync(message.Chat.Id, "input not recognized");
                }
            }
            else
            {
                await _botClient.SendTextMessageAsync(message.Chat.Id, "text only dude");
            }
        }

        private void SetSessionContext(long chatId, string chatUsername)
        {
            _sessionContext.ChatId = chatId;
            _sessionContext.ClientName = chatUsername;
            _sessionContext.ReportType = ReportType.File;
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