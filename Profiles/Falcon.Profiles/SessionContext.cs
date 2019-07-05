using System;

namespace Falcon.Profiles
{
    public class SessionContext
    {
        public Guid SessionId { get; set; }

        public string ClientName { get; set; }
    }
}