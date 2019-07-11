using System.Collections.Generic;

namespace Falcon.Profiles.Scan
{
    public abstract class ScanProfile : ITargetProfile, IToolProfile
    {
        public string Target { get; set; }

        public List<string> Tools { get; set; }

        public IContext Context { get; set; }
    }
}