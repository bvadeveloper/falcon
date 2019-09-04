namespace Falcon.Profiles.Telegram
{
    public class TelegramFileProfile : TelegramProfile
    {
        public byte[] File { get; set; }

        public string FileName { get; set; }
    }
}