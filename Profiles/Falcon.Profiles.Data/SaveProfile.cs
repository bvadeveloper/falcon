namespace Falcon.Profiles.Data
{
    public class SaveProfile : IProfile
    {
        public string Data { get; set; }
        
        public SessionContext Context { get; set; }
    }
}