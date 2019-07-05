namespace Falcon.Profiles.Data
{
    public class SaveProfile : ISessionContext
    {
        public string Data { get; set; }

        public SessionContext Context { get; set; }
    }
}