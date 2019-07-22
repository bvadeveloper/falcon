using System.Collections.Generic;

namespace Falcon.Profiles.Telegram
{
    public class TelegramProfile : IReportProfile
    {
        public IContext Context { get; set; }

        public string Target { get; set; }

        public List<ReportModel> Reports { get; set; }
    }

    public class TelegramTextProfile : TelegramProfile { }

    public class TelegramFileProfile : TelegramProfile
    {
        public byte[] ReportBytes { get; set; }

        public string FileName { get; set; }
    }
}