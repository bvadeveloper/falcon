using System;
using System.IO;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Falcon.Logging;
using Falcon.Profiles;
using Falcon.Profiles.Report;
using Falcon.Profiles.Telegram;
using Falcon.Reports;

namespace Falcon.Hosts.Report.Consumers
{
    public class ReportConsumer : IConsumeAsync<ReportProfile>
    {
        private readonly IBus _bus;
        private readonly IReportService _reportService;
        private readonly IJsonLogger _logger;

        public ReportConsumer(
            IBus bus,
            IReportService reportService,
            IJsonLogger<ReportConsumer> logger)
        {
            _bus = bus;
            _reportService = reportService;
            _logger = logger;
        }

        public async Task ConsumeAsync(ReportProfile profile)
        {
            switch (profile.Context)
            {
                case ApiContext apiContext:
                    throw new NotImplementedException(nameof(profile.Context));
                    break;

                case MessengerContext messengerContext:
                    await SaveFileAsync(profile, messengerContext);
                    await PublishTelegramProfileAsync(profile, messengerContext);
                    break;

                default:
                    throw new ArgumentException(nameof(profile.Context));
            }
        }

        private async Task SaveFileAsync(IReportProfile profile, MessengerContext messengerContext)
        {
            Directory.CreateDirectory("Report");
            var (fileName, reportBytes) = await _reportService.MakeFileReportAsync(profile.Target, profile.Reports);
            await File.WriteAllBytesAsync(Path.Combine("Report", fileName), reportBytes);
        }

        private async Task PublishTelegramProfileAsync(IReportProfile profile, IMessengerContext context)
        {
            switch (context.ReportType)
            {
                case ReportType.Text:
                    var message = await _reportService.MakeTextReportAsync(profile.Target, profile.Reports);
                    await SendTextReportAsync(profile, message);
                    break;

                case ReportType.File:
                    var (fileName, file) = await _reportService.MakeFileReportAsync(profile.Target, profile.Reports);
                    await SendFileReportAsync(profile, fileName, file);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task SendFileReportAsync(ITargetProfile profile, string fileName, byte[] file)
        {
            await _bus.PublishAsync(new TelegramFileProfile
            {
                Context = profile.Context,
                Target = profile.Target,
                File = file,
                FileName = fileName
            });
        }

        private async Task SendTextReportAsync(ITargetProfile profile, string message)
        {
            await _bus.PublishAsync(new TelegramMessageProfile
            {
                Context = profile.Context,
                Target = profile.Target,
                Message = message
            });
        }
    }
}