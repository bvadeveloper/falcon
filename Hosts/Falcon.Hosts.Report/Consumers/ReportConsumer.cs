using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Falcon.Profiles.Report;
using Falcon.Profiles.Telegram;

namespace Falcon.Hosts.Report.Consumers
{
    public class ReportConsumer : IConsumeAsync<ReportProfile>
    {
        private readonly IBus _bus;

        public ReportConsumer(IBus bus)
        {
            _bus = bus;
        }

        public async Task ConsumeAsync(ReportProfile profile)
        {
            await PublishTelegramProfile(profile);
        }

        private async Task PublishTelegramProfile(ReportProfile profile)
        {
            await _bus.PublishAsync(new TelegramProfile
            {
                Context = profile.Context,
                Target = profile.Target,
                Reports = profile.Reports,
            });
        }
    }
}