using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Falcon.Data.Redis;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Reports;
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
        private readonly ICacheService _cacheService;
        private readonly IJsonLogger _logger;

        private readonly TimeSpan _ttl = TimeSpan.FromHours(24);
        private static string MakeTelegramReportKey(long key) => $"telegram:report:{key}";

        public TelegramMessageHandler(
            IRequestManagementService processingService,
            ITelegramBotClient botClient,
            IJsonLogger<TelegramMessageHandler> logger,
            IMessengerContext sessionContext,
            ICacheService cacheService)
        {
            _processingService = processingService;
            _botClient = botClient;
            _sessionContext = sessionContext;
            _cacheService = cacheService;
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


            var commands = message.Text.Trim().Split(' ');
            var reportType = await GetReportType(commands, message);
            var text = commands.FirstOrDefault();

            switch (text)
            {
                case "/inline":
                    await _botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "report format?",
                        replyMarkup: MakeReportButtons());
                    break;
                default:
                    await ProcessRequest(text, message.Chat.Id, message.Chat.Username, reportType);
                    break;
            }
        }

        private async Task<ReportType> GetReportType(IReadOnlyList<string> commands, Message message)
        {
            ReportType reportType;

            if (commands.Count == 2)
            {
                Enum.TryParse(commands[1], true, out reportType);
            }
            else
            {
                reportType = await _cacheService.GetValueAsync<ReportType>(MakeTelegramReportKey(message.Chat.Id));
            }

            return reportType;
        }

        private async Task ProcessRequest(string text, long chatId, string userName, ReportType reportType)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                await _botClient.SendTextMessageAsync(chatId, "input not recognized");
                return;
            }

            await _botClient.SendChatActionAsync(chatId, ChatAction.Typing);

            _sessionContext.ChatId = chatId;
            _sessionContext.ClientName = userName;
            _sessionContext.ReportType = reportType;

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

        public async void Callback(object sender, CallbackQueryEventArgs e)
        {
            if (Enum.TryParse<ReportType>(e.CallbackQuery.Data, true, out var reportType))
            {
                await _cacheService.SetValueAsync(MakeTelegramReportKey(e.CallbackQuery.Message.Chat.Id),
                    reportType,
                    _ttl);
            }

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