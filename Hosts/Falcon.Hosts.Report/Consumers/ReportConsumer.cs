using System;
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
                    await PublishTelegramProfile(profile, messengerContext);
                    break;

                default:
                    throw new ArgumentException(nameof(profile.Context));
            }
        }

        private async Task PublishTelegramProfile(IReportProfile profile, IMessengerContext context)
        {
            switch (context.ReportType)
            {
                case ReportType.Text:
                    var reportText = await _reportService.MakeTextReportAsync(profile.Target, profile.Reports);
                    await SendTextReportAsync(profile, reportText);
                    break;

                case ReportType.File:
                    var (fileName, reportBytes) =
                        await _reportService.MakeFileReportAsync(profile.Target, profile.Reports);
                    await SendFileReportAsync(profile, fileName, reportBytes);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task SendFileReportAsync(ITargetProfile profile, string fileName, byte[] reportBytes)
        {
            await _bus.PublishAsync(new TelegramFileProfile
            {
                Context = profile.Context,
                Target = profile.Target,
                ReportBytes = reportBytes,
                FileName = fileName
            });
        }

        private async Task SendTextReportAsync(ITargetProfile profile, string reportText)
        {
            await _bus.PublishAsync(new TelegramTextProfile
            {
                Context = profile.Context,
                Target = profile.Target,
                ReportText = reportText
            });
        }
    }
}