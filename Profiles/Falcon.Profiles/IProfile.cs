namespace Falcon.Profiles
{
    public interface IProfile
    {
        SessionContext Context { get; set; }
    }
}