using System.Collections.Generic;

namespace Falcon.Profiles.Telegram
{
    public class TelegramProfile : IReportProfile
    {
        public SessionContext Context { get; set; }
        
        public string Target { get; set; }

        public List<ReportModel> Reports { get; set; }
    }
}