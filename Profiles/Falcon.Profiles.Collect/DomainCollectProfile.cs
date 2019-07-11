using System.Collections.Generic;

namespace Falcon.Profiles.Collect
{
    
    public class DomainCollectProfile : ITargetProfile, IToolProfile
    {
        public string Target { get; set; }

        public List<string> Tools { get; set; }

        public IContext Context { get; set; }
    }
}