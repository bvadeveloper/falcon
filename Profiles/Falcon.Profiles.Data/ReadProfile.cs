namespace Falcon.Profiles.Data
{
    public class ReadProfile : ISession
    {
        public string Request { get; set; }

        public IContext Context { get; set; }
    }
}