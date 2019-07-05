namespace Falcon.Profiles.Data
{
    public class ReadProfile : IProfile
    {
        public string Request { get; set; }

        public SessionContext Context { get; set; }
    }
}