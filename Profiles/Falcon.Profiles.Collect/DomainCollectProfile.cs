using System.Collections.Generic;

namespace Falcon.Profiles.Collect
{
    public class DomainCollectProfile : IProfile
    {
        public string Target { get; set; }

        public List<string> Tools { get; set; }

        public SessionContext Context { get; set; }
    }
}