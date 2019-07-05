using System.Collections.Generic;

namespace Falcon.Profiles.Scan
{
    public abstract class ScanProfile : IProfile
    {
        public string Target { get; set; }

        public List<string> Tools { get; set; }

        public SessionContext Context { get; set; }
    }
}