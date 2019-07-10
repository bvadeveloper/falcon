using System;
using System.Threading;
using System.Threading.Tasks;
using Falcon.Logging;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Falcon.Api.HostedServices
{
    public class MessengersHostedService : IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IJsonLogger _logger;

        public MessengersHostedService(ITelegramBotClient botClient, IJsonLogger<MessengersHostedService> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var me = await _botClient.GetMeAsync(cancellationToken);
            _botClient.StartReceiving(Array.Empty<UpdateType>(), cancellationToken);
            _logger.Information($"Messenger host started '{me.Username}'");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _botClient.StopReceiving();
            _logger.Information($"Messenger host stopped");
            return Task.CompletedTask;
        }
    }
}