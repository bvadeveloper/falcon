using System;
using System.Collections.Generic;
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
            _logger.Trace("Process report profile", profile);

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

        private async Task PublishTelegramProfile(ReportProfile profile, MessengerContext context)
        {
            switch (context.ReportType)
            {
                case ReportType.Text:
                    await SendTextReportAsync(profile);
                    break;
                case ReportType.Pdf:
                    var (fileName, reportBytes) = await MakePdfFileAsync(profile.Target, profile.Reports);
                    await SendPdfReportAsync(profile, fileName, reportBytes);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task<(string, byte[])> MakePdfFileAsync(string target, List<ReportModel> models)
        {
            var fileName = $"{target}_report_{DateTime.UtcNow:yyMMddHHmm}.pdf";
            var report = await _reportService.MakeReportAsync(ReportType.Pdf, fileName, models);

            return (fileName, report);
        }

        private async Task SendPdfReportAsync(ReportProfile profile, string fileName, byte[] reportBytes)
        {
            await _bus.PublishAsync(new TelegramFileProfile
            {
                Context = profile.Context,
                Target = profile.Target,
                ReportBytes = reportBytes,
                FileName = fileName
            });
        }

        private async Task SendTextReportAsync(IReportProfile profile)
        {
            await _bus.PublishAsync(new TelegramTextProfile
            {
                Context = profile.Context,
                Target = profile.Target,
                Reports = profile.Reports,
            });
        }
    }
}