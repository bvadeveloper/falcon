using System;

namespace Falcon.Profiles
{
    public interface ISessionContext
    {
        SessionContext Context { get; set; }
    }

    public class SessionContext
    {
        public Guid SessionId { get; set; }

        public string ClientName { get; set; }
    }
}