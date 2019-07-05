namespace Falcon.Profiles.Data
{
    public class ReadProfile : ISessionContext
    {
        public string Request { get; set; }

        public SessionContext Context { get; set; }
    }
}