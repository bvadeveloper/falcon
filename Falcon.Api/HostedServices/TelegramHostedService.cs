using System;
using System.Threading;
using System.Threading.Tasks;
using Falcon.Logging;
using Falcon.Messengers.Telegram;
using Falcon.Services.RequestManagement;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Falcon.Api.HostedServices
{
    public class TelegramHostedService : IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IMessageHandler _telegramMessageHandler;
        private readonly IJsonLogger _logger;

        public TelegramHostedService(
            ITelegramBotClient botClient,
            IMessageHandler messageHandler,
            IJsonLogger<TelegramHostedService> logger)
        {
            _botClient = botClient;
            _telegramMessageHandler = messageHandler;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _telegramMessageHandler.Subscribe(_botClient);
            _botClient.StartReceiving(Array.Empty<UpdateType>(), cancellationToken);
            _logger.Information($"Messenger host started");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _botClient.StopReceiving();
            _logger.Information($"Messenger host stopped");

            return Task.CompletedTask;
        }
    }
}